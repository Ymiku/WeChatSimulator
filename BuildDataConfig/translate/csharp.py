#!/usr/bin/env python
#encoding: utf-8
import os, re, sys, json
import os.path as p

DELTA_INDENT = ' '*4
SCRIPT_SHEET_NAME = 'SCRIPT_TEXT'

class ScriptEntity(object):
    def read(self, buffer):
        pass

    def dump(self, indent = None):
        return ''

class VarEntity(ScriptEntity):
    def __init__(self):
        self.modifiers = []
        self.value = None
        self.type = None
        self.name = None

    def read(self, line):
        pair = re.split(r'\s*=\s*', str(line).strip())
        list = re.split(r'\s+', pair[0])
        self.name = list[-1]
        del list[-1]
        self.type = list[-1]
        del list[-1]
        self.modifiers = list
        self.value = re.search(r'"([^\"]+)"', pair[1]).group(1)

    def dump(self, indent = None):
        if not indent:
            indent = ''
        return '%s%s %s %s = "%s";\n'%(indent, ' '.join(self.modifiers), self.type, self.name, self.value)

class ClassEntity(ScriptEntity):
    def __init__(self, namespace, definition):
        self.namespace = namespace
        pair = re.split(r'\s*:\s*', self.__strip(definition))
        self.supers = re.split(r'\s*,\s*', pair[1])
        self.modifiers = re.split(r'\s+', pair[0])
        self.name = self.modifiers[-1]
        del self.modifiers[-1]
        self.entities = []
    
    def __strip(self, line):
        return re.sub(r'[\r\n]+','', line).strip()

    def dump_text_map(self):
        text_map = {}
        for entity in self.entities:
            if not isinstance(entity, VarEntity):
                continue
            label = u'%s-%s'%(self.name, entity.name)
            text_map[label] = entity.value.decode('utf-8')
        return text_map
    def translate(self, text_map):
        for entity in self.entities:
            if not isinstance(entity, VarEntity):
                continue
            label = '%s-%s'%(self.name, entity.name)
            translated_value = text_map.get(label.decode('utf-8'))
            if translated_value:
                # print '%r %r %r'%(label, entity.value, translated_value)
                print '%s %s => %s'%(label, entity.value, translated_value.encode('utf-8'))
                entity.value = translated_value.encode('utf-8')

    def read(self, buffer):
        line, char = '', None
        while char == None or char != '':
            char = buffer.read(1)
            if char == ';':
                var = VarEntity()
                var.read(self.__strip(line))
                self.entities.append(var)
                line = ''
                continue
            if char == '/':
                if not line.lstrip():
                    note = CommentEntity()
                    note.read(buffer)
                    self.entities.append(note)
                    line = ''
                    continue
            if char == '}':
                if not line.strip():
                    return
            line += char

    def dump(self, indent = None):
        if not indent:
            indent = ''
        indent += DELTA_INDENT
        result = 'namespace %s\n{\n'%(self.namespace)
        result += '%s%s %s'%(indent, ' '.join(self.modifiers), self.name)
        if len(self.supers) > 0:
            result += ':' + ','.join(self.supers)
        result += '\n'
        result += '%s{\n'%(indent)
        for entity in self.entities:
            result += entity.dump(indent + DELTA_INDENT)
        result += '%s}\n'%(indent)
        result += '}\n'
        return result

class CommentEntity(ScriptEntity):
    def __init__(self):
        self.content = None

    def __read_until_newline(self, buffer):
        line, char = '', None
        while char == None or char != '':
            char = buffer.read(1)
            if char == '\n':
                return re.sub(r'^\/+','', line).strip()
            line += char

    def __read_until_pair_match(self, buffer):
        line, char = '', None
        while char == None or char != '':
            char = buffer.read(1)
            if char == '*':
                next = buffer.read(1)
                if next == '/':
                    line = re.sub(r'\n\s+', '\n', line)
                    return re.sub(r'\r', '', line[:-1].strip())
                buffer.seek(-1, os.SEEK_CUR)
            line += char

    def read(self, buffer):
        line, char = '', None
        while char == None or char != '':
            char = buffer.read(1)
            if char == '/':
                self.content = self.__read_until_newline(buffer)
                return
            if char == '*':
                self.content = self.__read_until_pair_match(buffer)
                return
    def dump(self, indent = None):
        if not indent:
            indent = ''
        if self.content.find('\n') >= 0:
            return '%s/*\n%s%s\n%s */\n'%(indent, indent + ' ', re.sub(r'\n', '\n%s '%(indent), self.content), indent)
        else:
            return '%s// %s\n'%(indent, self.content)

class ScriptTranslator(ScriptEntity):
    def __init__(self, script_path = None):
        if script_path and p.exists(script_path):
            self.script_path = p.abspath(script_path)
            with open(self.script_path, 'r') as fp:
                self.read(fp)

    def __strip(self, line):
        line = re.sub(r'^\W+', '', line)
        return re.sub(r'[\r\n]+$','', line).strip()
    
    def read(self, buffer):
        self.__usings = []
        self.__entities = []
        namespaces = []
        line, char = '', None
        while char == None or char != '':
            char = buffer.read(1)
            if char == '{':
                line = self.__strip(line)
                if re.search(r'^namespace', line):
                    namespaces.append(re.split(r'\s+', line)[-1])
                if re.search(r'\s+class\s+', line):
                    entity = ClassEntity('.'.join(namespaces), self.__strip(line))
                    entity.read(buffer)
                    self.__entities.append(entity)
                line = ''
                continue
            if char == '}':
                del namespaces[-1]
                line = ''
                continue
            if char == ';':
                line = self.__strip(line)
                if line and re.search(r'using', line):
                    self.__usings.append(line)
                line = ''
                continue
            if char == '/':
                if not line.lstrip():
                    note = CommentEntity()
                    note.read(buffer)
                    self.__entities.append(note)
                    line = ''
                    continue
            line += char

    def dump(self):
        result = ''
        for using in self.__usings:
            result += '%s;\n'%(using)
        namespaces = []
        for entity in self.__entities:
            result += entity.dump()
        return result

    def save(self):
        with open(self.script_path, 'w') as fp:
            fp.write(self.dump())
            fp.close()
        print 'TRANSLATE => %s'%(self.script_path)

    def json(self, compact = False):
        if compact:
            return json.dumps(self.dump_text_map(), sort_keys=True, ensure_ascii=False, separators=(',',':'))
        else:
            return json.dumps(self.dump_text_map(), sort_keys=True, ensure_ascii=False, indent=4)

    def dump_text_map(self):
        text_map = {}
        for entity in self.__entities:
            if not isinstance(entity, ClassEntity):
                continue
            text_map.update(entity.dump_text_map())
        return text_map

    def translate(self, text_map):
        for entity in self.__entities:
            if not isinstance(entity, ClassEntity):
                continue
            entity.translate(text_map)

def __script_list():
    root_dir = p.abspath(p.join(p.dirname(p.abspath(__file__)), '../..'))
    script_list = __get_script_paths()
    for n in range(len(script_list)):
        script_list[n] = p.join(root_dir, script_list[n])
    return script_list

def dump_text_map():
    text_map = {}
    for script_path in __script_list():
        translator = ScriptTranslator(script_path)
        text_map.update(translator.dump_text_map())
    return text_map

def translate(text_map):
    for script_path in __script_list():
        translator = ScriptTranslator(script_path)
        translator.translate(text_map)
        translator.save()

def __get_script_paths():
    script_list = []
    script_list.append('Assets/Plugins/Apollo/Scripts/Apollo/Dolphin/super/Core/DolphinInfoStringPartial.cs')
    return script_list

def dump():
    return json.dumps(dump_text_map(), ensure_ascii=False, sort_keys=True, indent=4)

if __name__ == '__main__':
    print dump()
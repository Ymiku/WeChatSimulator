#!/usr/bin/env bash

cd $(dirname ${0})
python collect.py
svn commit -m '自动提交增量汉字' database
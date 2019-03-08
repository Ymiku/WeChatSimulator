using System;

namespace TheNext.Moba.Logic
{
	/// <summary>
	/// 状态名称
	/// </summary>
	public class StatusType
    {
        public const string STUN 					= "stun";                       //眩晕 无法操控角色
		public const string ENSNARED 				= "ensnared"; 					//禁锢 无法移动
		public const string UNTARGETABLE 			= "untargetable"; 				//当前不能作为目标
		public const string SILENCED 				= "silenced";					//沉默 无法释放技能
		public const string INVINCIBLE 				= "invincible";                 //无敌 不能成为目标，不能收到伤害
		public const string TAUNT 					= "taunt"; 						//嘲讽，当A嘲讽B后，B的攻击目标会转为A
        public const string SUPPRESSION             = "suppression";                //压制
        public const string IN_COMBAT 				= "in_combat";					//处于战斗中
        public const string NOT_COST_MANA 			= "not_cost_mana";				//技能不消耗蓝
		public const string IGNORE_CONTROL 			= "ignore_control";				//免疫控制
        public const string IGNORE_JIANSU           = "ignore_jiansu";              //免疫减速
        public const string PUSH_AWAY 				= "PushAway";                   //冲锋		主动位移
        public const string Forced_Displacement     = "Forced_Displacement";        //受迫位移  	被动位移
        public const string STASIS                  = "stasis"; 				    //凝滞，金身
        public const string IN_HEAL                 = "in_heal";                    //治疗中，用于饰品治愈圣铃
        public const string TRNSFORM_BODY           = "transform_body";             //变身
        public const string Mark                    = "mark";                       //标记
        public const string BLEEDING                = "bleeding";                   //出血，流血     
        public const string IN_FLYING               = "in_flying";                  //飞行状态：可以移动，不能攻击，可以放技能     
        public const string FIX_WEAPON              = "fix_weapon";                 //固定武器
		public const string RESIST_ABILITY_DAMAGE 	= "re_ab_da";					//抵挡技能伤害
		public const string RESIST_ABILITY_BUFF 	= "re_ab_bu";					//抵挡技能BUFF
		public const string RESIST_AWARD_GOLD 		= "re_aw_go"; 					//抵挡奖励金币

        public const string ATTACK_FORBIDDEN        = "attack_forbidden";           //禁止普攻
        public const string DONT_STOP_MOVE          = "dont_stop_move";             //移动后不可停止
		public const string REFRESH_CD				= "refresh_cd";					//使用刷新CD标志

	    public const string RESIST_DAMAGE           = "resist_damage";              //抵挡所有伤害
        public const string FORCE_ATTACKABLE        = "foce_attackable";            //忽略无敌免疫等状态，强制可被攻击
        public const string JIAN_SHENG_UNTARGETABLE = "jiansheng_untargetable";     //剑圣的不可选中状态(不能施法，不能)
        public const string IN_PORTAL               = "in_portal";                  //在传送门里面
    }
}


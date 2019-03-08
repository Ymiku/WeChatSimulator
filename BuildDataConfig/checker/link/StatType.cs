
namespace TheNext.Moba.Logic
{
	/// <summary>
	/// 属性名称
	/// </summary>
	public class StatType
	{
		//血量，蓝量和英雄等级，不作为属性，只是在这里枚举
		public const string HEALTH = "health";
		public const string MANA = "mana";
		public const string LEVEL = "level";

		//进攻属性
		public const string ARMOR_PENETRATION = "armor_penetration"; 		//护甲穿透	固定指
		public const string ARMOR_PENETRATION_PERCENT = "armor_penetration_percent";//护甲穿透	//百分比
		public const string ARMOR_PENETRATION_PERCENT_ADD = "armor_penetration_percent_add";	//额外护甲穿透百分比
		public const string ATTACK_DAMAGE = "attack_damage"; 				//攻击强度  //体术   // 全攻击强度
		public const string EXTRA_ATTACK_DAMAGE = "extra_attack_damage";    //额外攻击强度
		public const string ATTACK_SPEED = "attack_speed"; 					//攻击速度
		public const string ATTACK_SPEED_BASE = "attack_speed_base";		//基础值，恒定不变
		public const string CRITICAL_CHANCE = "critical_chance"; 			//爆击几率
		public const string CRITICAL_DAMAGE = "critical_damage"; 			//爆击伤害
		public const string LIFE_STEAL = "life_steal"; 						//生命偷取
        public const string ATTACK_DISTANCE = "attack_distance";            //攻击距离
		public const string AOE_RANGE = "aoe_range";//aoe半径
		public const string AOE_ANGLE = "aoe_angle";//aoe角度
		public const string TARGET_SELECT_RANGE = "target_select_range";	//搜索半径
        public const string HERO_SEARCH_RANGE_ADDITION = "hero_search_range_addition";  // 英雄搜索半径增加量
        public const string HERO_FIND_PATH_RANGE = "hero_find_path_range";              // 英雄寻路半径
        public const string ATTACK_ADDTIONAL_PREPARE_TIME = "attack_additional_prepare_time"; //普攻附加前摇时间
        public const string ATTACK_DAMAGE_PERCENT = "attack_damage_percent";//普攻伤害衰减比例
	    public const string NON_ATTACK_DAMAGE_PERCENT = "non_attack_damage_percent";//非普攻伤害加成比例

		//防御属性
		//血量，实际的血量有多个影响因素
		public const string MAX_HEALTH = "max_health";						//最大血量
		public const string MIN_HEALTH = "min_health";
		public const string HEALTH_REGEN = "health_regen"; 					//血量恢复
		public const string CURRENT_PERCENT_HEALTH_REGEN = "current_percent_health_regen"; //基于当前血量的恢复比例
		public const string MAX_PERCENT_HEALTH_REGEN = "max_percent_health_regen";

		public const string HEALTH_DECAY = "health_decay";
		public const string CURRENT_PERCENT_HEALTH_DECAY = "current_perent_health_decay";
		public const string MAX_PERCENT_HEALTH_DECAY = "max_percent_health_decay";

		public const string HEALTH_MULTI = "health_multi";					//血量恢复的百分比
		public const string SHIELD = "shield"; 								//护盾
        public const string MAGIC_SHIELD = "magic_shield";                  //魔法护盾
        public const string PHYSIC_SHIELD = "physic_shield";                //物理护盾
		public const string INJURED = "injured";							//重伤效果

		public const string ARMOR = "armor"; 								//护甲
		public const string MAGIC_RESIST = "magic_resist"; 					//魔抗
		public const string RESISTANCE = "resistance";						//伤害减免
        public const string ATTACK_DECREASE = "attack_decrease";            //伤害衰减
		public const string ALL_BLOCK = "all_block";						//全伤害格挡
		public const string BLOCK = "block";								//格挡伤害值
		public const string BLOCK_PERCENT = "block_percent";				//格挡比例
		public const string STATE_LIFETIME_REDUCTION_PERCENT = "state_lifetime_reduction_percent";	//消极状态的持续时间缩短百分比
		public const string CONTROL_COOLDOWN_PERCENT = "control_cooldown_percent";//减控制状态CD时间

		//技能属性
		public const string ABILITY_POWER = "ability_power"; 				//技能强度			//忍术
		public const string ABILITY_COOLDOWN_PERCENT = "ability_cooldown_percent";			//技能cd缩短比例
		public const string ABILITY_COOLDOWN_REDUCTION = "ability_cooldown_reduction"; 		//冷却缩短时间
		public const string MAGIC_PENETRATION = "magic_penetration"; 		//魔法穿透
		public const string MAGIC_PENETRATION_PERCENT = "magic_penetration_percent";//魔法穿透 	//百分比
		public const string MAGIC_PENETRATION_PERCENT_ADD = "magic_penetration_percent_add";//额外魔法穿透 百分比
		public const string SPELL_VAMP = "spell_vamp"; 						//技能吸血

		//蓝 查克拉
		public const string MAX_MANA = "max_mana";							//最大蓝两 			//查克拉
		public const string MIN_MANA = "min_mana";
		public const string MANA_REGEN = "mana_regen";						//回蓝
		public const string CURRENT_PERCENT_MANA_REGEN = "current_percent_mana_regen";
		public const string MAX_PERCENT_MANA_REGEN = "max_percent_mana_regen";
        public const string RESOURCE_REGEN_PERCENT = "resource_regen_percent";

		//移动属性
		public const string MOVE_SPEED = "move_speed";						//移动速度
		public const string MOVE_SPEED_BONUS = "move_speed_bonus"; 			//移动速度加速系数，调整使用FixedAdjust
		public const string MOVE_SPEED_SLOW = "move_speed_slow";			//减速数值，调整使用FixedAdjust
		public const string MOVE_SPEED_SLOW_RESIST = "move_speed_slow_resist";//减速抗性，调整使用FixedAjust
        public const string TURN_SPEED_RATE = "turn_speed_rate";            //转身速度缩放系数

		//death reborn
		public const string REBORN_TIME_INTERVAL = "reborn_time_interval";
        public const string REBORN_TIME_REDUCTION_PERCENT = "reborn_time_reduction_percent";   //调整死亡复活时间的缩减百分比

		//死亡相关
		public const string GOLD_AWARD_ATTACKER = "gold_award_attacker";
		public const string GOLD_AWARD_RANGE = "gold_award_range";
		public const string GOLD_AWARD_TEAM = "gold_award_team";
		public const string EXP_ON_DEATH = "exp_on_death";
		public const string BONUS_GOLD_LAST_HIT = "bonus_gold_last_hit"; 	//最后一击的多余奖励
        public const string HEALTH_REGEN_ATTACKER_BUFF_ID = "health_regen_attacker_buff_id";//最後一擊的血量回復bufferID
        public const string GOLD_PROP_ADD = "atk_prop_add";	//金幣屬性加成
        public const string EXP_PROP_ADD = "exp_prop_add";	//經驗屬性加成

		public const string EXP_EXTRA = "exp_extra"; 		//每秒额外获得的经验
		public const string GOLD_EXTRA = "gold_extra"; 		//每秒额外获得的金币

        // buff 属性
        public const string BUFF_STAMINA ="stamina"; // 耐力
        
        //fow
        public const string VISIBLE_RANGE = "visible_range";
        //技能额外的施法范围
        public const string ABILITY_RANGE = "ability_range";
	}
}


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIFrameWork{
	public class GamePlayView : EnabledView {
		public Image hpImage;
		public Image energyImage;
		public Image shieldImage;
		public Image speedUp;
		private GamePlayContext _context;
		public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as GamePlayContext;
		}

		public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
		}

		public override void Excute ()
		{
			base.Excute ();

			//energyImage.fillAmount = Mathf.Lerp (energyImage.fillAmount,_playerPawn.energy/_playerPawn.maxEnergy,Time.deltaTime);
			//shieldImage.fillAmount = Mathf.Lerp (shieldImage.fillAmount,_playerPawn.hp/_playerPawn.maxShield,Time.deltaTime);
		}

		
		
	}
	public class GamePlayContext :BaseContext
	{
		public GamePlayContext() : base(UIType.GamePlay)
		{
		}
	}
}
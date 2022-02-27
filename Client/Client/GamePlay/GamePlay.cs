// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:GamePlay.cs
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using Client.Logic;
using Client.Net;
using Client.View;

namespace Client
{
	public class GamePlay
	{
		public GamePlay()
		{
		}

		public GameLogic logic = new GameLogic();
		public GameView view = new GameView();
		public GamePlayData gamePlayData = new GamePlayData();

		public void Init()
		{
			NetManager.Instance.RegisterCallBack("gamestart",OnGameStart);
			NetManager.Instance.RegisterCallBack("frame",OnFrame);
			NetManager.Instance.RegisterCallBack("gameover",OnGameOver);
		}

		private void OnGameStart(string s1, string s2)
		{
			logic.OnGameStart(s2);
		}

		private void OnGameOver(string arg1, string arg2)
		{
			logic.OnGameOver();
		}
		
		private void OnFrame(string arg1, string arg2)
		{
			logic.OnFrame(arg2);
		}

		public void Tick(double deltaTime)
		{
			logic.Tick(deltaTime);
			view.Tick(deltaTime);
		}

		public bool IsOver()
		{
			return logic.IsOver();
		}

		public void Release()
		{
			NetManager.Instance.UnRegisterCallBack("frame",OnFrame);
			NetManager.Instance.UnRegisterCallBack("gameover",OnGameOver);
			NetManager.Instance.UnRegisterCallBack("gamestart",OnGameStart);
		}
	}
}

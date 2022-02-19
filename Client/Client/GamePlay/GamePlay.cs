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
		NetManager netManager = new NetManager();

		public void InitNetWork()
		{
			netManager.Init(gamePlayData.playerId);
			netManager.RegisterCallBack("frame",OnFrame);
		}

		private void OnFrame(string arg1, string arg2)
		{
			logic.OnFrame(arg2);
		}

		public void Tick(double deltaTime)
		{
			netManager.Update(deltaTime);
			logic.Tick(deltaTime);
			view.Tick(deltaTime,netManager);
		}
	}
}

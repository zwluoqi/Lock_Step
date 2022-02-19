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
// * Created:2022/2/19
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using Server.Net;
using Server.ServerRunning.Frame;

namespace Server.ServerRunning
{
	public class GamePlay
	{
		public ServerUDPMgr serverUDPMgr = new ServerUDPMgr();
		public FrameManager frameManager = new FrameManager();
		public GamePlay()
		{
		}

		internal void InitNetWork()
		{
			serverUDPMgr.Init();
			serverUDPMgr.RegisterCallBack("ping", OnPing);
			serverUDPMgr.RegisterCallBack("frame", OnFrame);
		}

		private void OnFrame(string arg1, string arg2)
		{
			frameManager.OnFrame(arg2);
		}

		private void OnPing(string arg1, string arg2)
		{
			
		}

		public void Tick(double deltaTime)
		{
			serverUDPMgr.Update();
			frameManager.Tick(deltaTime, serverUDPMgr);
		}
	}
}

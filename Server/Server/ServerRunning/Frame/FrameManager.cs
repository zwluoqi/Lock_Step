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
// * Filename:FrameManager.cs
// * Created:2022/2/19
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using log4net;
using Server.Net;

namespace Server.ServerRunning.Frame
{
	public class FrameManager
	{
		public int curFrameCount = 0;
		public const int fps = 10;
		public double curFrameTime;
		public double timer;

		public FrameSyncManager frameSyncManager = new FrameSyncManager();


		public double spaceTimer
		{
			get
			{
				return 1.0 / fps;
			}
		}

		public void Tick(double deltaTime, ServerUDPMgr serverUDPMgr)
		{
			timer += deltaTime;
			if (timer > curFrameTime + spaceTimer)
			{
				curFrameTime += spaceTimer;
				_InnerTick(serverUDPMgr);
			}
		}

		internal void OnFrame(string arg2)
		{
			frameSyncManager.ReceiveFrameData(arg2);
		}

		private void _InnerTick(ServerUDPMgr serverUDPMgr)
		{
			curFrameCount++;
			//LogManager.GetLogger("logic").Debug("逻辑帧:" + curFrameCount);
			frameSyncManager.PushFrameData(curFrameCount, serverUDPMgr);
		}
	}
}

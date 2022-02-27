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

		public FrameManager frameManager = new FrameManager();
		public GamePlay()
		{
		}

		public void Init()
		{
			ServerUDPMgr.Instance.RegisterCallBack("ping", OnPing);
			ServerUDPMgr.Instance.RegisterCallBack("frame", OnFrame);
			ServerUDPMgr.Instance.RegisterCallBack("connect", OnConnect);
		}



		public void Release()
		{
			ServerUDPMgr.Instance.UnRegisterCallBack("ping", OnPing);
			ServerUDPMgr.Instance.UnRegisterCallBack("frame", OnFrame);
			ServerUDPMgr.Instance.UnRegisterCallBack("connect", OnConnect);
		}

		internal bool IsOver()
		{
			return frameManager.IsOver();
		}

		private void OnConnect(string arg1, string arg2)
		{
			frameManager.OnConnect();
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
			frameManager.Tick(deltaTime);
		}
	}
}

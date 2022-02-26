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
// * Filename:Program.cs
// * Created:2022/2/19
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.IO;
using System.Threading;
using log4net;

using log4net.Config;
using Server.Net;
using Server.ServerRunning;

namespace Server
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			
			Console.WriteLine("Hello Server!");
			ServerUDPMgr.Instance.Init();

			GamePlay gamePlay = new GamePlay();
			gamePlay.Init();
			while (true)
			{
				ServerUDPMgr.Instance.Update();
				gamePlay.Tick(0.033);
				if (gamePlay.IsOver())
				{
					gamePlay.Release();
					gamePlay = new GamePlay();
					gamePlay.Init();
				}
				Thread.Sleep(33);
			}
		}
	}
}

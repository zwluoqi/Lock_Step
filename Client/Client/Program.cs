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
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Threading;
using Client.Net;

namespace Client
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello Client!");
			int playerId = (new Random(System.DateTime.Now.Millisecond)).Next();
			NetManager.Instance.Init(playerId);

			GamePlay gamePlay = new GamePlay();
			gamePlay.Init();
			while (true)
			{
				NetManager.Instance.Update(0.033);
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

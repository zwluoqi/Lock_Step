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
// * Filename:GameView.cs
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;
using Client.Net;

namespace Client.View
{
	public class GameView
	{
		public CommondDataManager commondDataManager = new CommondDataManager();
		public FrameUploadManager frameUploadManager = new FrameUploadManager();

		public List<GameViewActor> actors = new List<GameViewActor>();

		internal void Tick(double deltaTime)
		{
			var commondList = commondDataManager.GetCommondList();
			foreach(var commond in commondList)
			{
				var commondAPI = GetCommondAPI(commond.commondType);
				commondAPI.Process(commond);
			}
			foreach (var actor in actors)
			{
				actor.Tick(deltaTime);
			}
			
			frameUploadManager.PushFrameData();
		}

		private CommondAPI GetCommondAPI(int commondType)
		{
			throw new NotImplementedException();
		}
	}
}

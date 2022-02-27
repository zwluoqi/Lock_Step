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

		public double moveTimer = 0;
		public double stopTimer = 0;
		public double attackTimer = 0;
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

			//TEST INPUT
			TEST_INPUT(deltaTime);
			
			frameUploadManager.PushFrameData();

		}

		private void TEST_INPUT(double deltaTime)
		{
			moveTimer += deltaTime;
			stopTimer += deltaTime;
			attackTimer += deltaTime;
			if (moveTimer > 5)
			{
				frameUploadManager.SendMove();
				moveTimer = 0;
			}
			
			if (stopTimer > 7)
			{
				stopTimer = 0;
				frameUploadManager.SendStop();
			}

			if (attackTimer > 10)
			{
				attackTimer = 0;
				frameUploadManager.SendAttack();
			}		}

		private CommondAPI GetCommondAPI(int commondType)
		{
			throw new NotImplementedException();
		}
	}
}

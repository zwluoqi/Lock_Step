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
// * Filename:ViewNetManager.cs
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using Client.Logic;
using Client.Net;
using Newtonsoft.Json.Linq;

namespace Client.View
{
	public class FrameUploadManager
	{

		public FrameInputData sendCache = new FrameInputData();
		
		public void PushFrameData()
		{
			if (sendCache.frameOpeDatas.Count == 0)
			{
				return;
			}
			sendCache.frameCount = 0;
			JObject jObject = new JObject();
			jObject["frame"] = Newtonsoft.Json.JsonConvert.SerializeObject(sendCache);
			NetManager.Instance.SendMsg("frame",jObject.ToString());
			sendCache.Clear();
		}

		public void SendMove()
		{
			FrameMoveData frameMoveData = new FrameMoveData();
			frameMoveData.frameType = 1;
			frameMoveData.moveDir = 1;
			sendCache.AddFrame(frameMoveData);
		}

		public void SendStop()
		{
			FrameStopData frameStopData = new FrameStopData();
			frameStopData.frameType = 2;
			sendCache.AddFrame(frameStopData);
		}

		public void SendAttack()
		{
			FrameAttackData frameAttack = new FrameAttackData();
			frameAttack.frameType = 3;
			frameAttack.attackId = 1;
			sendCache.AddFrame(frameAttack);
		}
	}
}

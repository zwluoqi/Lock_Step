﻿// /*
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
		
		public void PushFrameData(NetManager netManager)
		{
			sendCache.frameCount = 0;
			JObject jObject = new JObject();
			jObject["frame"] = Newtonsoft.Json.JsonConvert.SerializeObject(sendCache);
			netManager.SendMsg("frame",jObject.ToString());
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
			// JObject jObject = new JObject();
			// jObject["frame"] = Newtonsoft.Json.JsonConvert.SerializeObject(frameStopData);
			// netManager.SendMsg("frameinput",jObject.ToString());
			sendCache.AddFrame(frameStopData);
		}

		public void SendAttack()
		{
			FrameAttackData frameAttack = new FrameAttackData();
			frameAttack.frameType = 3;
			frameAttack.attackId = 1;
			// JObject jObject = new JObject();
			// jObject["frame"] = Newtonsoft.Json.JsonConvert.SerializeObject(frameAttack);
			// netManager.SendMsg("frameinput",jObject.ToString());
			sendCache.AddFrame(frameAttack);
		}
	}
}
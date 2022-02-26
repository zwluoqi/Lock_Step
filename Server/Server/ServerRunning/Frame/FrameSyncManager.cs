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
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Server.Net;

namespace Server.ServerRunning.Frame
{
	public class FrameSyncManager
	{

		FrameInputData sendCache = new FrameInputData();

		private List<FrameInputData> cacheFrames = new List<FrameInputData>();

		public void PushFrameData(int frameCount)
		{
			sendCache.frameCount = frameCount;
			JObject jObject = new JObject();
			var str = Newtonsoft.Json.JsonConvert.SerializeObject(sendCache);
			jObject["frame"] = str;
			ServerUDPMgr.Instance.SendMsgToAll("frame", jObject.ToString());
			cacheFrames.Add(sendCache);
			sendCache = new FrameInputData();
		}

		public void ReceiveFrameData(string json)
		{
			var receiveFrame = Newtonsoft.Json.JsonConvert.DeserializeObject<FrameInputData>(json);
			sendCache.AddFrames(receiveFrame);
		}

		internal void PushGameOverData()
		{
			JObject jObject = new JObject();
			jObject["winplayer"] = "none";
			ServerUDPMgr.Instance.SendMsgToAll("gameover", jObject.ToString());
		}
	}
}

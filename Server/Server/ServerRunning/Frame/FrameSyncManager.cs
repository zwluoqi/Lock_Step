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

using Newtonsoft.Json.Linq;
using Server.Net;

namespace Server.ServerRunning.Frame
{
	public class FrameSyncManager
	{

		public FrameInputData sendCache = new FrameInputData();

		public void PushFrameData(int frameCount,ServerUDPMgr netManager)
		{
			sendCache.frameCount = frameCount;
			JObject jObject = new JObject();
			var str = Newtonsoft.Json.JsonConvert.SerializeObject(sendCache);
			jObject["frame"] = str;
			netManager.SendMsgToAll("frame", jObject.ToString());
		}

		public void ReceiveFrameData(string json)
		{
			var receiveFrame = Newtonsoft.Json.JsonConvert.DeserializeObject<FrameInputData>(json);
			receiveFrame.AddFrames(receiveFrame);
		}
	}
}

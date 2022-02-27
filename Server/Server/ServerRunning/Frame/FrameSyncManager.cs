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

		FrameInputDatas cacheFrames = new FrameInputDatas();


		/// <summary>
		/// 客户端发送过来的帧数据
		/// </summary>
		/// <param name="json"></param>
		public void ReceiveFrameData(string json)
		{
			JObject jObject = JObject.Parse(json);
			var frame = (string)jObject["frame"];
			var receiveFrame = Newtonsoft.Json.JsonConvert.DeserializeObject<FrameInputData>(frame);
			sendCache.AddFrames(receiveFrame);
		}


		/// <summary>
		/// 推送服务器搜集到的所有帧数据
		/// </summary>
		/// <param name="frameCount"></param>
		public void PushFrameData(int frameCount)
		{
			sendCache.frameCount = frameCount;
			JObject jObject = new JObject();
			var str = Newtonsoft.Json.JsonConvert.SerializeObject(sendCache);
			jObject["frame"] = str;
			ServerUDPMgr.Instance.SendMsgToAll("frame", jObject.ToString());
			cacheFrames.curFrameDatas.Add(sendCache);
			sendCache = new FrameInputData();
		}


		/// <summary>
		/// 服务器战斗结束
		/// </summary>
		internal void PushGameOverData()
		{
			JObject jObject = new JObject();
			jObject["winplayer"] = "none";
			ServerUDPMgr.Instance.SendMsgToAll("gameover", jObject.ToString());
		}

		/// <summary>
		/// 传送当前游戏的所有帧数据
		/// </summary>
		internal void PushGameStartData()
		{
			JObject jObject = new JObject();
			var str = Newtonsoft.Json.JsonConvert.SerializeObject(cacheFrames);
			jObject["frames"] = str;
			ServerUDPMgr.Instance.SendMsgToAll("gamestart", jObject.ToString());
		}
	}
}

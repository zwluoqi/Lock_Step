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
// * Filename:GameLogic.cs
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;
using log4net;
using Newtonsoft.Json.Linq;

namespace Client.Logic
{
	public class GameLogic
	{

		int curFrameCount = 0;
		const int fps = 10;
		double curFrameTime;
		double timer;

		FrameDownLoadManager frameDataManager = new FrameDownLoadManager();
		List<GameLogicActor> logicActorList = new List<GameLogicActor>();
		private bool isOver = false;

		public double spaceTimer
		{
			get
			{
				return 1.0 / fps;
			}
		}
		public GameLogic()
		{

		}

		internal void Tick(double deltaTime)
		{
			if (isOver)
			{
				return;
			}
			var lastFrameDataCount = frameDataManager.GetLastFrameDataCount();
			if(lastFrameDataCount == 0)
			{
				return;
			}
			//帧数据与客户端逻辑帧必须是一致的
			if(lastFrameDataCount != curFrameCount+1)
			{
				isOver = true;
				LogManager.GetLogger("logic").Error("帧混乱");
				// throw new Exception("帧混乱");
			}

			double realDeltaTime = deltaTime;
			//客户端延迟4秒以上，倍率2
			if (lastFrameDataCount - curFrameCount > 4*fps)
			{
				realDeltaTime = deltaTime * 2;
			}
			//客户端延迟2秒，倍率1.5
			else if (lastFrameDataCount - curFrameCount > 2*fps)
			{
				realDeltaTime = deltaTime * 1.5;
			}
			else
			{
				//
			}

			timer += realDeltaTime;
			if (timer > curFrameTime + spaceTimer)
			{
				curFrameTime += spaceTimer;
				timer = curFrameTime;
				_InnerTick();
			}
		}

		private void _InnerTick()
		{
			curFrameCount++;
			LogManager.GetLogger("logic").Debug("逻辑帧:" + curFrameCount);
			var frameInputData = frameDataManager.GetFrameDataList(curFrameCount);
			foreach(var frameData in frameInputData.frameOpeDatas)
			{
				var frameAPI = GetFrameProcessAPI(frameData.frameType);
				frameAPI.Process(frameData);
			}
			foreach(var actor in logicActorList)
			{
				actor.Tick();
			}
		}

		private FrameAPI GetFrameProcessAPI(int frameType)
		{
			throw new NotImplementedException();
		}

		public void OnFrame(string json)
		{
			JObject jObject = JObject.Parse(json);
			var frame = (string)jObject["frame"];
			var receiveFrame = Newtonsoft.Json.JsonConvert.DeserializeObject<FrameInputData>(frame);
			frameDataManager.OnFrame(receiveFrame);
		}

		public bool IsOver()
		{
			return isOver;
		}

		public void OnGameOver()
		{
			isOver = true;
		}
	}
}

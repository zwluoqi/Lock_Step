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

		public static double spaceTimer
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
			var lastFrameCounter = frameDataManager.GetLastFrameDataCount();
			var firstFrameCounter = frameDataManager.GetFirstFrameDataCount();
			if(lastFrameCounter == 0)
			{
				return;
			}
			//帧数据与客户端逻辑帧必须是一致的
			if(firstFrameCounter != curFrameCount+1)
			{
				isOver = true;
				LogManager.GetLogger("logic").Error("帧混乱");
				throw new Exception("帧混乱");
				return;
			}

			double realDeltaTime = deltaTime;
			//客户端延迟4秒以上，倍率8
			if (lastFrameCounter - curFrameCount > 8*fps)
			{
				realDeltaTime = deltaTime * 8;
			}
			//客户端延迟2秒，倍率2
			else if (lastFrameCounter - curFrameCount > 4*fps)
			{
				realDeltaTime = deltaTime * 4;
			}
			else if (lastFrameCounter - curFrameCount > 2*fps)
			{
				realDeltaTime = deltaTime * 2;
			}
			else if(lastFrameCounter - curFrameCount < fps/2)
			{
				realDeltaTime = deltaTime;
			}
			else
			{
				realDeltaTime = deltaTime * 1.5;
			}

			timer += realDeltaTime;
			while (timer > curFrameTime + spaceTimer && lastFrameCounter!=0)
			{
				curFrameTime += spaceTimer;
				_InnerTick();
				lastFrameCounter = frameDataManager.GetLastFrameDataCount();
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
			//TODO
			return new FrameAPI();
		}

		public void OnGameStart(string json)
		{
			JObject jObject = JObject.Parse(json);
			var frames = (string)jObject["frames"];
			var receiveFrames = Newtonsoft.Json.JsonConvert.DeserializeObject<FrameInputDatas>(frames);
			frameDataManager.OnFrames(receiveFrames);
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

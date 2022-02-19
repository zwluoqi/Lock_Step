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
// * Filename:GameLogic.cs
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;

namespace Client.Logic
{
	public class GameLogic
	{

		public int curFrameCount = 0;
		public const int fps = 10;
		public double curFrameTime;
		public double timer;

		public FrameDownLoadManager frameDataManager = new FrameDownLoadManager();
		public List<GameLogicActor> logicActorList = new List<GameLogicActor>();

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
			var lastFrameDataCount = frameDataManager.GetLastFrameDataCount();
			if(lastFrameDataCount == 0)
			{
				return;
			}
			if(lastFrameDataCount < curFrameCount)
			{
				throw new Exception("帧混乱");
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
			Debug.Log("逻辑帧:" + curFrameCount);
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
	}
}
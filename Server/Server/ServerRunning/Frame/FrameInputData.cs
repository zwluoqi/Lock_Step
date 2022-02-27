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
// * Filename:GamePlay.cs
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;

namespace Server.ServerRunning.Frame
{

	[System.Serializable]
	public class FrameInputDatas
	{
		public List<FrameInputData> curFrameDatas = new List<FrameInputData>();
	}

	[System.Serializable]
	public class FrameInputData
	{
		public List<FrameOpeData> frameOpeDatas = new List<FrameOpeData>();
		public int frameCount;

		public void AddFrame(FrameOpeData frameMoveData)
		{
			frameOpeDatas.Add(frameMoveData);
		}

		public void AddFrames(FrameInputData receiveFrame)
		{
			foreach(var frame in receiveFrame.frameOpeDatas)
			{
				frameOpeDatas.Add(frame);
			}
		}

		public void ClearFrames()
		{
			frameOpeDatas.Clear();
		}
	}

	[System.Serializable]
	public class FrameOpeData
	{
		public int frameType;
	}

	public enum FrameTYpe
	{
		NONE,
		MOVE,
		STOP,
		ATTACK,
	}


	public class FrameMoveData : FrameOpeData
	{
		public int moveDir;//1-360;
	}

	public class FrameStopData : FrameOpeData
	{

	}
	public class FrameAttackData : FrameOpeData
	{
		public int attackId;
	}
}
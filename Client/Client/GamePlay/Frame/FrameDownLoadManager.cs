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
// * Filename:GamePlay.cs
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
	public class FrameDownLoadManager
	{
		List<FrameInputData> curFrameDatas = new List<FrameInputData>();

		
		internal int GetFirstFrameDataCount()
		{
			if (curFrameDatas.Count > 0)
			{
				return curFrameDatas[0].frameCount;
			}
			else
			{
				return 0;
			}
		}
		
		internal int GetLastFrameDataCount()
		{
			if (curFrameDatas.Count > 0)
			{
				return curFrameDatas[curFrameDatas.Count-1].frameCount;
			}
			else
			{
				return 0;
			}
		}

		internal FrameInputData GetFrameDataList(int curFrameCount)
		{
			var frame = curFrameDatas[0];
			curFrameDatas.RemoveAt(0);
			return frame;
		}

		public void OnFrame(FrameInputData frameInputData)
		{
			curFrameDatas.Add(frameInputData);
		}

		public void OnFrames(FrameInputDatas receiveFrames)
		{
			curFrameDatas.AddRange(receiveFrames.curFrameDatas);
		}
	}
}
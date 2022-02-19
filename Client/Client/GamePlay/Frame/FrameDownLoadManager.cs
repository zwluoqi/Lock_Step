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
		Queue<FrameInputData> curFrameDatas = new Queue<FrameInputData>();

		internal int GetLastFrameDataCount()
		{
			if (curFrameDatas.Count > 0)
			{
				return curFrameDatas.Peek().frameCount;
			}
			else
			{
				return 0;
			}
		}

		internal FrameInputData GetFrameDataList(int curFrameCount)
		{
			return curFrameDatas.Dequeue();
		}
	}
}
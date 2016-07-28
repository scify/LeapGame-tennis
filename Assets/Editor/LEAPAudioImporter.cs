
/**
 * Copyright 2016 , SciFY NPO - http://www.scify.org
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
/**
 * The file of LEAPAudioImporter class. 
 * 
 * This file holds the declaration and 
 * implementation of the LEAPAudioImporter class.
 * 
 * @file LEAPAudioImporter.cs
 * @version 1.0
 * @date 21/01/2015 (dd/mm/yyyy)
 * @author Konstantinos Drossos
 * @copyright SciFY distributed as is under Apache v2 License.
 */
using UnityEngine;
using UnityEditor;


/**
 * Class to handle the import of audio files.
 * 
 * This class pre-process the audio files and sets
 * the desired settings for them. 
 * 
 * @see AssetPostprocessor
 * @author Konstantinos Drossos
 */
public class LEAPAudioImporter : AssetPostprocessor {

	/**
	 * Audio pre-processing.
	 * 
	 * This private method is called on audio pre-processing
	 * when each audio file is imported on the editor. 
	 * 
	 * @author Konstantinos Drossos
	 */
	private void OnPreprocessAudio() {
		AudioImporter importer = (AudioImporter)assetImporter;

		importer.forceToMono = false;
		//importer.threeD = true; //not necessary for this game
	}



	/**
	 * Audio post-processing.
	 * 
	 * This private method is called for the post-processing
	 * of audio assets. It accepts the AudioClip object to be
	 * processed and operates on it. Currently is just a stub
	 * method serving for future needs. 
	 * 
	 * @param audioClip - the AudioClip object to be processed (AudioClip)
	 * @author Konstantinos Drossos
	 */
	private void OnPostprocessAudio(AudioClip audioClip) {
	}
}

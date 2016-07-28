﻿
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectStorage {

	public const string ALL = "*";
	
	public ObjectStorage() {
	}
	
	public abstract string update(Object o, string category, string id);
	
	public abstract Object load(string category, string id);
	
	public abstract List<Object> loadAll(string category);
	
	public abstract List<string> list(string category);
	
	public abstract bool delete(string category, string id);
	
}
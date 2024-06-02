/*
 * 
				for (int i = 0; i < roadObject[k].roadPoints.Length; i++) {
					if (roadPoints [i].state == RoadPoint.State.Out) {
						if (roadPoints[i].direction == RoadPoint.Direction.ZMinus && k == 0) {
							minusConnection.Add (roadObject [k].roadPoints [i]);
						}else if (roadPoints[i].direction == RoadPoint.Direction.ZPlus && k == 1) {
							plusConnection.Add (roadObject [k].roadPoints[i]);
						}else if (roadPoints[i].direction == RoadPoint.Direction.XMinus && k == 2) {
							minusConnection.Add (roadObject [k].roadPoints[i]);
						}
					} else {
						if (roadPoints[i].direction == RoadPoint.Direction.ZPlus && k == 0) {
							plusConnection.Add (roadObject [k].roadPoints[i]);
						}else if (roadPoints[i].direction == RoadPoint.Direction.ZMinus && k == 1) {
							minusConnection.Add (roadObject [k].roadPoints [i]);
						} 
					}
				}


				for (int i = 0; i < roadPoints.Length; i++) {
					if (roadPoints [i].state == RoadPoint.State.In) {
						if (roadPoints [i].direction == RoadPoint.Direction.ZMinus && k == 0) {
							for (int j = 0; j < minusConnection.Count; j++) {
								minusConnection[j].connectedPoints.Add(roadPoints[i].gameObject);
							}
						}else if (roadPoints [i].direction == RoadPoint.Direction.ZPlus && k == 1) {
							for (int j = 0; j < plusConnection.Count; j++) {
								plusConnection[j].connectedPoints.Add(roadPoints[i].gameObject);
							}
						}
					} else {
						if (roadPoints[i].direction == RoadPoint.Direction.ZPlus && k == 0) {
							for (int j = 0; j < plusConnection.Count; j++) {
								roadPoints[i].connectedPoints.Add(plusConnection[j].gameObject);
							}
						}else if (roadPoints[i].direction == RoadPoint.Direction.ZMinus && k == 1) {
							for (int j = 0; j < minusConnection.Count; j++) {
								roadPoints[i].connectedPoints.Add(minusConnection[j].gameObject);
							}
						}
					}
				}

				for (int i = 0; i < roadObject[k].roadPoints.Length; i++) {
					if (roadPoints [i].state == RoadPoint.State.Out) {
						if (roadPoints[i].direction == RoadPoint.Direction.ZMinus && k == 0) {
							roadObject[k].roadPoints[i].connectedPoints.Clear();
						}
						if (roadPoints[i].direction == RoadPoint.Direction.ZPlus && k == 1) {
							roadObject[k].roadPoints[i].connectedPoints.Clear();
						}
					}else if (roadPoints [i].state == RoadPoint.State.In) {
					}
				}
 * 
 * 
 */
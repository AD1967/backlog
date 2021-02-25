using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class GameBot : MonoBehaviour
{
    public struct cord{
        public int a, b; 
        public bool what_to_do; // false - красим, true - апгрейдим
		public cord(int x, int y, bool z){
			a = x;
			b = y;
			what_to_do = z;
		}
    }
    private static int max = 0;
    public static List<cord> max_out;
    public static List<cord> ans = new List<cord>(); 
    public static int frame = -1, flag = -1;
    private static int[] Y = {0, 0, -1, 1};
    private static int[] X = {1, -1, 0, 0};
    void Update(){ 
    	if(ans.Count > 0 && flag == -1)
    		flag = 0; 

    	if(flag >= 0){
    		Thread.Sleep(600);
    		Print(ans[flag]);
    		flag++;
    	}

    	if(flag == ans.Count){
    		flag = -1; 
    		ans = new List<cord>();
    		MapGenerator.turn = false;
    	}

    	if(frame >= 0)
    		frame++;
    	if(frame >= 5){
    		BotStep();
    		frame = -1;
    	}
    }

    public static void BotStep(){
        //("StepBot");
        max = -1;
        Step(new List<cord>(), 0, 0);
        // исполняем лучший вариант.

        foreach(cord i in max_out){
			ans.Add(i);
		}
    }   
	private static void Print(cord i){
		if(i.what_to_do){
                MapGenerator.map[i.a, i.b].level++;     
				MapGenerator.players[1]++;
			}
            else{
				MapGenerator.players[1] += MapGenerator.map[i.a, i.b].level + 1;
				if(MapGenerator.map[i.a, i.b].owner == 0)
					MapGenerator.players[0] -= (MapGenerator.map[i.a, i.b].level + 1);
                MapGenerator.map[i.a, i.b].owner = (MapGenerator.turn ? 1 : 0);
			}
            MapGenerator.turn_count--;
	 }

    private static void Step(List < cord > steps, int points, int priority){
        if(steps.Count == MapGenerator.turn_count){
            if(points + priority >= max){
                max = points + priority; 
				max_out = new List <cord>();
				foreach(cord i in steps)
					max_out.Add(i);
			}
            return;
        }
		for(int i = 0; i <  MapGenerator.n; i++){
			for(int j = 0; j <  MapGenerator.m; j++){
				if(MapGenerator.map[i, j].owner == 1){
					if(MapGenerator.map[i, j].level < SquareChanger.max_level){
						MapGenerator.map[i, j].level++;
						steps.Add(new cord(i , j, true));
						Step(steps, points + 1, priority);
						steps.RemoveAt(steps.Count - 1);
						MapGenerator.map[i, j].level--;
					}
				}
				else{
					int MX = -1;
					if(MapGenerator.type == 6){
						for(int k = 0; k < 4; k++){ // Проходимся по соседним "своим" клеткам и находим ту, у которой максимальный уровень
	            			int x = i + X[k], y = j + Y[k];
	            			if(x >= 0 && x < MapGenerator.n && y >= 0 && y < MapGenerator.m && MapGenerator.map[i, j].owner == 1)
	               				MX = Math.Max(MX, MapGenerator.map[x, y].level + 1);
	        			}
	       				int tp = (j % 2 == 0 ? 1 : -1);
	        			for(int k = -1; k <= 1; k += 2){
	            			int x = i + tp, y = j + k;
	            			if(x >= 0 && x < MapGenerator.n && y >= 0 && y < MapGenerator.m && MapGenerator.map[i, j].owner == 1)
	                			MX = Math.Max(MX, MapGenerator.map[x, y].level + 1);
	        			}
					}else{
						for(int k = -1; k <= 1; k += 2){ // Проходимся по соседним "своим" клеткам и находим ту, у которой максимальный уровень
							if(k + i >= 0 && k + i < MapGenerator.n && MapGenerator.map[k + i, j].owner == 1)
								MX = Math.Max(MX, MapGenerator.map[k + i, j].level + 1);
							if(k + j >= 0 && k + j < MapGenerator.m && MapGenerator.map[i, j + k].owner == 1)
								MX = Math.Max(MX, MapGenerator.map[i, j + k].level + 1);
						}
					}
					if(MX >= MapGenerator.map[i, j].level + 1){
						int was = MapGenerator.map[i, j].owner, cof = 1;
						if(was == 0)
							cof = 2;
						MapGenerator.map[i,j].owner = 1;
						steps.Add(new cord(i , j, false));
						Step(steps, points + (MapGenerator.map[i,j].level + 1) * cof, priority + cof);
						steps.RemoveAt(steps.Count - 1);
						MapGenerator.map[i,j].owner = was;
					}
				}
			}
		}

    } 
}
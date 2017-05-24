using UnityEngine;
using System.Collections;

public class ListIterator 
{

    public static int GetPositionIndex(int length, int item, string direction) {
    	if (direction == "up") {
    		if (item == 0) {
    			item = length - 1;
    		}
    		else {
    			item -= 1;
    		}
    	}

    	if (direction == "down") {
    		if (item == length - 1) {
    			item = 0;
    		}
    		else {
    			item += 1;
    		}
    	}

    	if (direction == "right") {
    		if (item == 0) {
    			item = length - 1;
    		}
    		else {
    			item -= 1;
    		}
    	}

    	if (direction == "left") {
    		if (item == length - 1) {
    			item = 0;
    		}
    		else {
    			item += 1;
    		}
    	}

    	return item;
    }

}

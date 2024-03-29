using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void TileInit(){
        
    }
    void OnTileClicked(){
        // logic implemented here and board manager
        //is some tile already clicked
            //if yes, this needs to be same type as that
                //if not same, return or switch active
        // is this location next to previously clicked
            //if not return or switch active
        //is this the third selected?
            //if yes, clear, etc
            //if not, just add to selected
    }
    void TileSelected(){
        //todo : rename
        //this is called from manager if this tile is okay to interact with
    }
    void TileUnselected(){

    }
    void TileCleared(){}

}

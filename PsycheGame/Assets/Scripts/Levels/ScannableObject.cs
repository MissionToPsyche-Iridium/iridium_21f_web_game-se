using UnityEngine;

// The interface all objects that wish to be scanned by the probe should
// implement, IN ADDITION TO SETTING THE GAME OBJECT LAYER TO 'Scannable'
public interface ScannableObject {

    // Function called every time the probe scans this scannable object
    // until the function [IsScanned] returns true
    public void Scan();

    // Function returning the progress of a scan on this scannable object
    // if this scan progress is completed it indicates to the probe that
    // the objects [Scan] function no longer needs to be called
    public Progress ScanProgress { get; }

    // This bridges the gap between the scannable object interface and
    // the unity monobehavoir class allowing an intsance of a scannable
    // object to be treated like a GameObject
    public GameObject GameObject { get; }

/*----------------------------------------------------------------------------*/
// The following functions are used by UI to display info about this scannable//
// object, at some point it may be best to move this to its own interface     //
// separate of scanning logic. For now though we only have to display objects //
// of type scannable object                                                   //
/*----------------------------------------------------------------------------*/

    // Return a short string description of this scanned object
    // upon scanning
    public string Description { get; }

    // Return an image of this scannable object upon scanning
    public Sprite Image { get; }

}

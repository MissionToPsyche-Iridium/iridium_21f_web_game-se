// The interface all objects that wish to be scanned by the probe should
// implement, IN ADDITION TO SETTING THE GAME OBJECT LAYER TO 'Scannable'
public interface ScannableObject {

    // Function called every time the probe scans this scannable object
    // until the function [IsScanned] returns true
    public void Scan();

    // When this function returns a value of [true] it indicates to the
    // probe that the object has been successfully scanned and the
    // [Scan] function no longer needs to be called
    public bool IsScanned { get; }

}

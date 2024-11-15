using System;

// A generic record representing progress on a scale of 0 to 100
// where 0 is the low value and 100 reprsents completion. Progress
// is clamped between 0 and 100 so any attempt to create progress
// outside of this range will become bounded.
public record Progress(float Value) {

    public static readonly float NO_PROGRESS = 0;
    public static readonly float COMPLETE_PROGRESS = 100;

    public Progress incr(float n) { return new Progress(Value + n); } // In MOST cases its advisable to multiply 'n' by [Time.deltaTime]
    public float Value { get; } = Math.Clamp(Value, 0, 100);
    public bool isComplete = Value >= COMPLETE_PROGRESS;
    public override string ToString() => $"{Value}%";

}
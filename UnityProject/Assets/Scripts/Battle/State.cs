

// - Dice, Enemy State Class
public class State
{
    // - State type
    enum STATE_TYPE
    { 
        STATE_NORMAL = 0,
        STATE_PRESSED = 1,
        STATE_RELEASED = 2,
        STATE_CHECKED = 3,
        STATE_DISABLED = 4,
        STATE_CHECKED_PRESSED = 5,
        STATE_CHECKED_RELEASED = 6,
        STATE_CHECKED_CHECKED = 7,
        STATE_CHECKED_DISABLED = 8,
        STATE_END
    }

}

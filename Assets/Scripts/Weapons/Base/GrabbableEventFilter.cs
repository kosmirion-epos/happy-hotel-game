using Autohand;
using System.Collections.Generic;
using System.Linq;

public abstract class GrabbableEventFilter : ExtendedBehaviour
{
    public virtual bool RejectsGrab(Hand hand, Grabbable grabbable) => false;
    public virtual bool RejectsRelease(Hand hand, Grabbable grabbable) => false;
    public virtual bool RejectsSqueeze(Hand hand, Grabbable grabbable) => false;
    public virtual bool RejectsUnsqueeze(Hand hand, Grabbable grabbable) => false;
    public virtual bool RejectsHighlight(Hand hand, Grabbable grabbable) => false;
    public virtual bool RejectsUnhighlight(Hand hand, Grabbable grabbable) => false;
    public virtual bool RejectsJointBreak(Hand hand, Grabbable grabbable) => false;

    public static bool AnyRejectsGrab(Hand hand, Grabbable grabbable, IEnumerable<GrabbableEventFilter> filters)
        => filters.Any(f => f.RejectsGrab(hand, grabbable));
    public static bool AnyRejectsRelease(Hand hand, Grabbable grabbable, IEnumerable<GrabbableEventFilter> filters)
        => filters.Any(f => f.RejectsRelease(hand, grabbable));
    public static bool AnyRejectsSqueeze(Hand hand, Grabbable grabbable, IEnumerable<GrabbableEventFilter> filters)
        => filters.Any(f => f.RejectsSqueeze(hand, grabbable));
    public static bool AnyRejectsUnsqueeze(Hand hand, Grabbable grabbable, IEnumerable<GrabbableEventFilter> filters)
        => filters.Any(f => f.RejectsUnsqueeze(hand, grabbable));
    public static bool AnyRejectsHighlight(Hand hand, Grabbable grabbable, IEnumerable<GrabbableEventFilter> filters)
        => filters.Any(f => f.RejectsHighlight(hand, grabbable));
    public static bool AnyRejectsUnhighlight(Hand hand, Grabbable grabbable, IEnumerable<GrabbableEventFilter> filters)
        => filters.Any(f => f.RejectsUnhighlight(hand, grabbable));
    public static bool AnyRejectsJointBreak(Hand hand, Grabbable grabbable, IEnumerable<GrabbableEventFilter> filters)
        => filters.Any(f => f.RejectsJointBreak(hand, grabbable));
}

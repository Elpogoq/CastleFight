﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Effect
{
    // Need to remaster the "isActive" part
    public override bool OnApply(Attackable target)
    {
        target.isActive = false;
        return true;
    }

    public override bool OnRemove(Attackable target)
    {
        target.isActive = true;
        return true;
    }

    public override bool OnTick(Attackable target)
    {
        return false;
    }
}

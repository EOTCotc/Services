﻿using ApeVolo.Common.Caches.Redis.Abstractions;

namespace ApeVolo.Common.Caches.Redis.Attributes;

public class SubscribeDelayAttribute : TopicAttribute
{
    public SubscribeDelayAttribute(string name) : base(name)
    {
    }
}
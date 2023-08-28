using Content.Shared.Chat;
using Content.Shared.Emoting;
using Content.Shared.Hands;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Maps;
using Content.Shared.Popups;
using Content.Shared.Sound.Components;
using Content.Shared.Throwing;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.ADT;

[UsedImplicitly]
public abstract class SharedEmitEmoteSystem : EntitySystem
{

    [Dependency] protected readonly SharedChatSystem ChatSystem = default!;
    public override void Initialize()
    {
        base.Initialize();
    }

}

using Robust.Shared.Audio;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Changed14.Fuckable;

[RegisterComponent, AutoGenerateComponentState, AutoGenerateComponentPause, NetworkedComponent]
public sealed partial class FuckableComponent : Component
{
    [DataField]
    public SoundSpecifier FuckableCumSound = new SoundPathSpecifier("/Audio/Voice/Human/malescream_1.ogg");

    [DataField]
    public SoundSpecifier FurryCumSound = new SoundPathSpecifier("/Audio/Voice/Vulpkanin/howl.ogg");

    [DataField, AutoNetworkedField]
    public ProtoId<ReagentPrototype> ReagentId = "FurryJuiceWhite";

    [DataField]
    public string Solution = "stomach";

    [DataField, AutoNetworkedField]
    public FixedPoint2 Quantity = 25;
}

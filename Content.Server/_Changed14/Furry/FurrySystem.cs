using Content.Server.DoAfter;
using Content.Shared.DoAfter;
using Content.Shared.Hands.Components;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Content.Shared.Changed14.Fuckable;
using Content.Shared.Tag;
using Robust.Shared.Audio.Systems;
using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Server.Chemistry.EntitySystems;

using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Content.Shared.Chemistry.Components.SolutionManager;

namespace Content.Server.Changed14.Fuckable;

public sealed class FuckableSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FuckableComponent, GetVerbsEvent<ActivationVerb>>(OnActivationVerb);
        SubscribeLocalEvent<FuckableComponent, FuckDoAfterEvent>(OnFuckDoAfter);
    }

    private void OnFuckDoAfter(EntityUid uid, FuckableComponent comp, ref FuckDoAfterEvent args)
    {

        if (args.Cancelled)
            return;

        _audio.PlayPvs(comp.FurryCumSound, args.User);
        _audio.PlayPvs(comp.FuckableCumSound, args.User);

        if (!_solutionContainer.TryGetInjectableSolution(uid, out var injectable, out _))
            return;
        _solutionContainer.TryAddReagent(injectable.Value, comp.ReagentId, 5);

    }

    private void OnActivationVerb(EntityUid uid, FuckableComponent comp, ref GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        if (!_tag.HasTag(args.User, "Furry"))
            return;

        var user = args.User;

        var verb = new ActivationVerb()
        {
            Act = () => HandleClimb(uid, user),
            Text = Loc.GetString("Чпокнуть"),
            Message = Loc.GetString("ОХ АХ"),
        };

        args.Verbs.Add(verb);
    }

    private void HandleClimb(EntityUid uid, EntityUid user)
    {
        var doAfterArgs = new DoAfterArgs(EntityManager, user, TimeSpan.FromSeconds(3), new FuckDoAfterEvent(), uid, uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false,
        };

        _doAfter.TryStartDoAfter(doAfterArgs);

    }

}

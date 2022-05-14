using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class NewCustomRuleTile : RuleTile<NewCustomRuleTile.Neighbor> {
    public bool customField;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Empty = 3;
        public const int AnyTile = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {

        if (tile is RuleOverrideTile ot)
            tile = ot.m_InstanceTile;

        switch (neighbor)
        {
            case Neighbor.This: return tile == this;
            case Neighbor.NotThis: return tile != this && tile != null;
            case Neighbor.Empty: return tile == null;
            case Neighbor.AnyTile: return tile != null;
        }
        return true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{



    public static class QPath
    {

        public static T[] FindPath<T>( IQPathWorld world, IQPathUnit unit, T startTile, T endTile,
            CostEstimateDelegate costEstimateFunc) where T : IQPathTile
        {
            //Debug.Log("QPath::FindPath");
            //Debug.Log(world);
           // Debug.Log(unit);
           // Debug.Log(startTile);
           // Debug.Log(endTile);

            if ( world == null || unit == null || startTile == null || endTile == null )
            {
                //Debug.LogError("null values passed to QPath::FindPath");
                return null;
            }

            // Call on our actual path solver


            QPath_AStar<T> resolver = new QPath_AStar<T>( world, unit, startTile, endTile, costEstimateFunc );

            resolver.DoWork();

            return resolver.GetList();
        }
    }

    public delegate float CostEstimateDelegate(IQPathTile a, IQPathTile b);
}
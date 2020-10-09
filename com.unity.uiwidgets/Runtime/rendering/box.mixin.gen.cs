using System;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using UnityEngine;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.ui;

namespace Unity.UIWidgets.rendering {

 
    public abstract class
        RenderBoxContainerDefaultsMixinContainerRenderObjectMixinRenderBox<ChildType, ParentDataType>
        : ContainerRenderObjectMixinRenderBox<ChildType, ParentDataType>
        where ChildType : RenderBox
        where ParentDataType : ContainerParentDataMixinBoxParentData<ChildType> {
        
        public float? defaultComputeDistanceToFirstActualBaseline(TextBaseline baseline) {
            var child = firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                float? result = child.getDistanceToActualBaseline(baseline);
                if (result != null) {
                    return result.Value + childParentData.offset.dy;
                }

                child = childParentData.nextSibling;
            }

            return null;
        }

        public float? defaultComputeDistanceToHighestActualBaseline(TextBaseline baseline) {
            float? result = null;
            var child = firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                float? candidate = child.getDistanceToActualBaseline(baseline);
                if (candidate != null) {
                    candidate += childParentData.offset.dy;
                    if (result != null) {
                        result = Mathf.Min(result.Value, candidate.Value);
                    } else {
                        result = candidate;
                    }
                }

                child = childParentData.nextSibling;
            }

            return result;
        }

        public void defaultPaint(PaintingContext context, Offset offset) {
            var child = firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                context.paintChild(child, childParentData.offset + offset);
                child = childParentData.nextSibling;
            }
        }

       public bool defaultHitTestChildren(BoxHitTestResult result, Offset position) {
            // the x, y parameters have the top left of the node's box as the origin4
            ChildType child = lastChild;
            while (child != null) {
                ParentDataType childParentData = child.parentData as ParentDataType;
                bool isHit = result.addWithPaintOffset(
                    offset: childParentData.offset,
                    position: position,
                    hitTest: (BoxHitTestResult boxHitTestResult, Offset transformed) => {
                    D.assert(transformed == position - childParentData.offset);
                    return child.hitTest(boxHitTestResult, position: transformed);
                }
                );
                if (isHit)
                    return true;
                child = childParentData.previousSibling;
            }
            return false;
        }

        public List<ChildType> getChildrenAsList() {
            var result = new List<ChildType>();
            var child = firstChild;
            while (child != null) {
                var childParentData = (ParentDataType) child.parentData;
                result.Add(child);
                child = childParentData.nextSibling;
            }

            return result;
        }
    }


}
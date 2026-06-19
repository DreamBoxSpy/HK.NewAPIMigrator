using NewAPIMigrator.Migrator;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable

namespace NewAPIMigrator.Runtime
{
    // Token: 0x020004D9 RID: 1241
    [RedirectType]
    public static class AExtensions_
    {
        // Token: 0x06001B6D RID: 7021 RVA: 0x00083724 File Offset: 0x00081924
        public static Selectable GetFirstInteractable(this Selectable start)
        {
            if (start == null)
            {
                return null;
            }
            if (start.interactable)
            {
                return start;
            }
            return start.navigation.selectOnDown.GetFirstInteractable();
        }

        // Token: 0x06001B6E RID: 7022 RVA: 0x00083759 File Offset: 0x00081959
        public static void PlayOnSource(this AudioClip self, AudioSource source, float pitchMin = 1f, float pitchMax = 1f)
        {
            if (self && source)
            {
                source.pitch = global::UnityEngine.Random.Range(pitchMin, pitchMax);
                source.PlayOneShot(self);
            }
        }

        // Token: 0x06001B6F RID: 7023 RVA: 0x00083780 File Offset: 0x00081980
        public static void SetActiveChildren(this GameObject self, bool value)
        {
            int childCount = self.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                self.transform.GetChild(i).gameObject.SetActive(value);
            }
        }

        // Token: 0x06001B70 RID: 7024 RVA: 0x000837BC File Offset: 0x000819BC
        public static void SetActiveWithChildren(this MeshRenderer self, bool value)
        {
            if (self.transform.childCount > 0)
            {
                MeshRenderer[] componentsInChildren = self.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].enabled = value;
                }
                return;
            }
            self.enabled = value;
        }

        // Token: 0x06001B71 RID: 7025 RVA: 0x00083800 File Offset: 0x00081A00
        public static bool HasParameter(this Animator self, string paramName, AnimatorControllerParameterType? type = null)
        {
            foreach (AnimatorControllerParameter animatorControllerParameter in self.parameters)
            {
                if (animatorControllerParameter.name == paramName)
                {
                    if (type != null)
                    {
                        AnimatorControllerParameterType type2 = animatorControllerParameter.type;
                        AnimatorControllerParameterType? animatorControllerParameterType = type;
                        if (!((type2 == animatorControllerParameterType.GetValueOrDefault()) & (animatorControllerParameterType != null)))
                        {
                            goto IL_0043;
                        }
                    }
                    return true;
                }
                IL_0043:;
            }
            return false;
        }

        // Token: 0x06001B72 RID: 7026 RVA: 0x0008385B File Offset: 0x00081A5B
        public static IEnumerator PlayAnimWait(this tk2dSpriteAnimator self, string anim)
        {
            tk2dSpriteAnimationClip clipByName = self.GetClipByName(anim);
            self.Play(clipByName);
            yield return new WaitForSeconds(clipByName.Duration);
            yield return new WaitForEndOfFrame();
            yield break;
        }

        // Token: 0x06001B73 RID: 7027 RVA: 0x00083874 File Offset: 0x00081A74
        public static float PlayAnimGetTime(this tk2dSpriteAnimator self, string anim)
        {
            tk2dSpriteAnimationClip clipByName = self.GetClipByName(anim);
            self.Play(clipByName);
            return clipByName.Duration;
        }

        // Token: 0x06001B74 RID: 7028 RVA: 0x00083898 File Offset: 0x00081A98
        public static Vector3 MultiplyElements(this Vector3 self, Vector3 other)
        {
            Vector3 vector = self;
            vector.x *= other.x;
            vector.y *= other.y;
            vector.z *= other.z;
            return vector;
        }

        // Token: 0x06001B75 RID: 7029 RVA: 0x000838DC File Offset: 0x00081ADC
        public static Vector2 MultiplyElements(this Vector2 self, Vector2 other)
        {
            Vector2 vector = self;
            vector.x *= other.x;
            vector.y *= other.y;
            return vector;
        }

        // Token: 0x06001B76 RID: 7030 RVA: 0x0008390E File Offset: 0x00081B0E
        public static void SetPositionX(this Transform t, float newX)
        {
            t.position = new Vector3(newX, t.position.y, t.position.z);
        }

        // Token: 0x06001B77 RID: 7031 RVA: 0x00083932 File Offset: 0x00081B32
        public static void SetPositionY(this Transform t, float newY)
        {
            t.position = new Vector3(t.position.x, newY, t.position.z);
        }

        // Token: 0x06001B78 RID: 7032 RVA: 0x00083956 File Offset: 0x00081B56
        public static void SetPositionZ(this Transform t, float newZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, newZ);
        }

        // Token: 0x06001B79 RID: 7033 RVA: 0x0008397A File Offset: 0x00081B7A
        public static void SetPosition2D(this Transform t, float x, float y)
        {
            t.position = new Vector3(x, y, t.position.z);
        }

        // Token: 0x06001B7A RID: 7034 RVA: 0x00083994 File Offset: 0x00081B94
        public static void SetPosition2D(this Transform t, Vector2 position)
        {
            t.position = new Vector3(position.x, position.y, t.position.z);
        }

        // Token: 0x06001B7B RID: 7035 RVA: 0x000839B8 File Offset: 0x00081BB8
        public static void SetPosition3D(this Transform t, float x, float y, float z)
        {
            t.position = new Vector3(x, y, z);
        }

        // Token: 0x06001B7C RID: 7036 RVA: 0x000839C8 File Offset: 0x00081BC8
        public static void SetScaleX(this Transform t, float newXScale)
        {
            t.localScale = new Vector3(newXScale, t.localScale.y, t.localScale.z);
        }

        // Token: 0x06001B7D RID: 7037 RVA: 0x000839EC File Offset: 0x00081BEC
        public static void SetScaleY(this Transform t, float newYScale)
        {
            t.localScale = new Vector3(t.localScale.x, newYScale, t.localScale.z);
        }

        // Token: 0x06001B7E RID: 7038 RVA: 0x00083A10 File Offset: 0x00081C10
        public static void SetScaleZ(this Transform t, float newZScale)
        {
            t.localScale = new Vector3(t.localScale.x, t.localScale.y, newZScale);
        }

        // Token: 0x06001B7F RID: 7039 RVA: 0x00083A34 File Offset: 0x00081C34
        public static void SetRotationZ(this Transform t, float newZRotation)
        {
            t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, newZRotation);
        }

        // Token: 0x06001B80 RID: 7040 RVA: 0x00083A58 File Offset: 0x00081C58
        public static void SetScaleMatching(this Transform t, float newScale)
        {
            t.localScale = new Vector3(newScale, newScale, newScale);
        }

        // Token: 0x06001B81 RID: 7041 RVA: 0x00083A68 File Offset: 0x00081C68
        public static float GetPositionX(this Transform t)
        {
            return t.position.x;
        }

        // Token: 0x06001B82 RID: 7042 RVA: 0x00083A75 File Offset: 0x00081C75
        public static float GetPositionY(this Transform t)
        {
            return t.position.y;
        }

        // Token: 0x06001B83 RID: 7043 RVA: 0x00083A82 File Offset: 0x00081C82
        public static float GetPositionZ(this Transform t)
        {
            return t.position.z;
        }

        // Token: 0x06001B84 RID: 7044 RVA: 0x00083A8F File Offset: 0x00081C8F
        public static float GetScaleX(this Transform t)
        {
            return t.localScale.x;
        }

        // Token: 0x06001B85 RID: 7045 RVA: 0x00083A9C File Offset: 0x00081C9C
        public static float GetScaleY(this Transform t)
        {
            return t.localScale.y;
        }

        // Token: 0x06001B86 RID: 7046 RVA: 0x00083AA9 File Offset: 0x00081CA9
        public static float GetScaleZ(this Transform t)
        {
            return t.localScale.z;
        }

        // Token: 0x06001B87 RID: 7047 RVA: 0x00083AB6 File Offset: 0x00081CB6
        public static float GetRotation2D(this Transform t)
        {
            return t.localEulerAngles.z;
        }

        // Token: 0x06001B88 RID: 7048 RVA: 0x00083AC4 File Offset: 0x00081CC4
        public static void SetRotation2D(this Transform t, float rotation)
        {
            Vector3 eulerAngles = t.eulerAngles;
            eulerAngles.z = rotation;
            t.eulerAngles = eulerAngles;
        }

        // Token: 0x06001B89 RID: 7049 RVA: 0x00083AE8 File Offset: 0x00081CE8
        public static bool IsAny(this string value, params string[] others)
        {
            foreach (string text in others)
            {
                if (value == text)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReelRoller : MonoBehaviour
{
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private RectTransform RectContainer;

    [SerializeField] private float SlotSpacing;
    [SerializeField] private Transform BottomSlotIntendedTransform;
    [SerializeField] private float MoveDuration;

    private float currentY;
    private int slotCount = 0;
    
    private Vector2 basePosition; // Store the base position after FixPosition
    private int totalReelSlots = 0; // Total slots in the original reel (excluding extras)
    private int currentSlotOffset = 0;
    private bool isMoving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentY = RectContainer.position.y;
        
        // Same as other code
        // -- Generate instances
        // -- Map images to those instances
        // -- same sequence as instances, but extra 3 at the top, same as bottom 3
        // When those 3 are displayed, move transform back to original position
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateSlots(List<Sprite> reel)
    {
        totalReelSlots = reel.Count; // Store original reel size
        
        foreach (var sprite in reel)
        {
            var slot = Instantiate(SlotPrefab, RectContainer);
            var slotRect = slot.GetComponent<RectTransform>();

            slot.GetComponent<Image>().sprite = sprite;
            // Pivot should be (0.5, 0) for upward stacking
            slotRect.anchorMin = new Vector2(0.5f, 0);
            slotRect.anchorMax = new Vector2(0.5f, 0);
            slotRect.pivot = new Vector2(0.5f, 0);

            // Position relative to bottom of parent
            float yPos = slotCount * SlotSpacing;
            slotRect.anchoredPosition = new Vector2(0, yPos);
            slotRect.sizeDelta = new Vector2(1.3f, 1.3f);

            slotCount++;
        }
        
        // Generate 3 more, starting from the first
        for (int i = 0; i < 3; i++)
        {
            var slot = Instantiate(SlotPrefab, RectContainer);
            var slotRect = slot.GetComponent<RectTransform>();

            slot.GetComponent<Image>().sprite = reel[i];
            // Pivot should be (0.5, 0) for upward stacking
            slotRect.anchorMin = new Vector2(0.5f, 0);
            slotRect.anchorMax = new Vector2(0.5f, 0);
            slotRect.pivot = new Vector2(0.5f, 0);

            // Position relative to bottom of parent
            float yPos = slotCount * SlotSpacing;
            slotRect.anchoredPosition = new Vector2(0, yPos);
            slotRect.sizeDelta = new Vector2(1.3f, 1.3f);
            slotCount++;
        }
        
        FixPosition();
    }

    void FixPosition()
    {
        Debug.Log($"Bottom Slot Intended Pos: {BottomSlotIntendedTransform.position}");
        Vector3 bottomPos = RectContainer.GetChild(0).transform.position;
        
        // Get difference between the child's pos and the intended transform pos
        Vector3 diff = bottomPos - BottomSlotIntendedTransform.position;
        
        // Shift entire reel by that difference
        RectContainer.position -= diff;
        
        // Store the base position in anchored coordinates for consistency
        basePosition = RectContainer.anchoredPosition;
        
        Debug.Log($"Bottom child pos now: {RectContainer.GetChild(0).transform.position}");
    }
    public async Task AnimateReelDownAsync(int numSlots)
    {
        if (isMoving) return;

        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(MoveReelDown(numSlots, tcs));
        await tcs.Task;
    }

    private IEnumerator MoveReelDown(int numSlots, TaskCompletionSource<bool> tcs)
    {
        isMoving = true;
        float slotDistance = RectContainer.localScale.y * SlotSpacing;
        float speedPerSlot = MoveDuration; // Duration per slot
        
        // Check if this move will exceed the threshold
        if (currentSlotOffset + numSlots >= totalReelSlots)
        {
            // Calculate how many slots we can move before hitting the threshold
            int slotsToMove = totalReelSlots - currentSlotOffset;
            int remainingSlots = numSlots - slotsToMove;
            
            // Move to the threshold first - duration based on actual slots moved
            if (slotsToMove > 0)
            {
                Vector2 startPos = RectContainer.anchoredPosition;
                Vector2 endPos = startPos - new Vector2(0, slotDistance * slotsToMove);
                yield return StartCoroutine(AnimateToPosition(startPos, endPos, speedPerSlot * slotsToMove));
            }
            
            // Reset position
            ResetReelPosition();
            
            // Continue with remaining slots - duration based on remaining slots
            if (remainingSlots > 0)
            {
                Vector2 newStartPos = RectContainer.anchoredPosition;
                Vector2 newEndPos = newStartPos - new Vector2(0, slotDistance * remainingSlots);
                yield return StartCoroutine(AnimateToPosition(newStartPos, newEndPos, speedPerSlot * remainingSlots));
                currentSlotOffset = remainingSlots;
            }
        }
        else
        {
            // Normal movement
            Vector2 startPos = RectContainer.anchoredPosition;
            Vector2 endPos = startPos - new Vector2(0, slotDistance * numSlots);
            yield return StartCoroutine(AnimateToPosition(startPos, endPos, speedPerSlot * numSlots));
            currentSlotOffset += numSlots;
        }
        
        isMoving = false;
        tcs.SetResult(true);
    }

    private IEnumerator AnimateToPosition(Vector2 startPos, Vector2 endPos, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            RectContainer.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Snap to exact position
        RectContainer.anchoredPosition = endPos;
    }
    
    private void ResetReelPosition()
    {
        // Reset to base position (this creates the infinite scroll effect)
        RectContainer.anchoredPosition = basePosition;
        currentSlotOffset = 0;
        
        Debug.Log("Reel position reset to prevent infinite scrolling");
    }

    public void MoveTo(int index)
    {
        ResetReelPosition();
        RectContainer.anchoredPosition -= new Vector2(0, index * SlotSpacing * RectContainer.localScale.y);
        currentSlotOffset = index;
    }
    
}

using UnityEngine;
using TMPro;
using YG;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShopManager : MonoBehaviour
{
    public SoundManager soundManager;

    [Header("Goods")]
    public List<Goods> defaultBackgrounds = new(); // Данные из инспектора (для первого запуска)
    public List<Goods> defaultItems = new(); // Данные из инспектора (для первого запуска)

    [Header("State Sprites")]
    public Sprite closedSprite;
    public Sprite availableSprite;
    public Sprite selectedSprite;

    [Header("UI References")]
    public Transform backgroundsContainer;
    public Transform itemsContainer;
    public GameObject itemPrefab;
    public Image[] uiElements; // UI элементы (кнопки, панели и т.п.)
    public Button[] uiButtons; // Кнопки, которые нужно окрашивать

    [Header("Background Settings")]
    public float fadeDuration = 0.5f;
    public SpriteRenderer backgroundRenderer;

    private int selectedIndexBackgrounds = 0;
    private int selectedIndexItems = 0;

    void Start()
    {
        // Подписываемся на событие загрузки данных
        YG2.onGetSDKData += OnGetData;

        // Если данные уже загружены, инициализируем магазин
        if (YG2.saves != null)
        {
            InitializeData();
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        YG2.onGetSDKData -= OnGetData;
    }

    private void OnGetData()
    {
        // Инициализируем данные после загрузки
        InitializeData();
    }

    private void InitializeData()
    {
        // Если списки пусты (первый запуск), используем данные из инспектора
        if (YG2.saves.backgrounds == null || YG2.saves.backgrounds.Count == 0)
        {
            YG2.saves.backgrounds = new List<Goods>(defaultBackgrounds);
            YG2.saves.items = new List<Goods>(defaultItems);
            YG2.saves.selectedBackgroundIndex = 0; // Устанавливаем фон с индексом 0 при первом запуске
            YG2.saves.selectedItemSkinIndex = 0; // Устанавливаем скин с индексом 0 при первом запуске
            YG2.SaveProgress(); // Сохраняем начальные данные
        }

        // Проверяем, что списки не пусты
        if (YG2.saves.backgrounds == null || YG2.saves.backgrounds.Count == 0)
        {
            Debug.LogError("Список backgrounds пуст! Проверьте данные в инспекторе.");
            return;
        }

        if (backgroundRenderer == null)
        {
            Debug.LogError("Background Renderer не назначен! Проверьте инспектор.");
            return;
        }

        // Устанавливаем сохранённые индексы
        selectedIndexBackgrounds = YG2.saves.selectedBackgroundIndex;
        selectedIndexItems = YG2.saves.selectedItemSkinIndex;

        // Устанавливаем фон
        SetBackgroundById(selectedIndexBackgrounds);

        // Инициализируем магазин
        CreateGoods(YG2.saves.backgrounds, backgroundsContainer, true);
        CreateGoods(YG2.saves.items, itemsContainer, false);

        // Обновляем визуал для обоих списков
        UpdateAllGoodsStates(YG2.saves.backgrounds, selectedIndexBackgrounds);
        UpdateAllGoodsStates(YG2.saves.items, selectedIndexItems);
    }

    private void CreateGoods(List<Goods> goodsList, Transform container, bool isBackground)
    {
        for (int i = 0; i < goodsList.Count; i++)
        {
            Goods goods = goodsList[i];
            GameObject newItem = Instantiate(itemPrefab, container);

            InitializeGoodsUI(goods, newItem);
            SetupGoodsButton(goods, newItem, i, isBackground);
            UpdateGoodsState(goods, i, isBackground ? selectedIndexBackgrounds : selectedIndexItems);
        }
    }

    private void InitializeGoodsUI(Goods goods, GameObject newItem)
    {
        goods.uiImage = newItem.transform.Find("ItemImage")?.GetComponent<Image>();
        goods.uiText = newItem.transform.Find("CostText")?.GetComponent<TextMeshProUGUI>();
        Image availabilityImage = newItem.transform.Find("AvailabilityImage")?.GetComponent<Image>();

        if (goods.uiImage != null) goods.uiImage.sprite = goods.sprite;
        if (availabilityImage != null) goods.uiImage = availabilityImage;
    }

    private void SetupGoodsButton(Goods goods, GameObject newItem, int index, bool isBackground)
    {
        newItem.GetComponent<Button>().onClick.AddListener(() => OnGoodsClick(index, isBackground));
    }

    public void OnGoodsClick(int index, bool isBackground)
    {
        List<Goods> goodsList = isBackground ? YG2.saves.backgrounds : YG2.saves.items;
        ref int selectedIndex = ref (isBackground ? ref selectedIndexBackgrounds : ref selectedIndexItems);

        if (index < 0 || index >= goodsList.Count) return;

        Goods clickedGoods = goodsList[index];

        if (TryPurchaseGoods(clickedGoods))
        {
            soundManager.PlayClickSound();
            selectedIndex = index;

            if (isBackground)
            {
                YG2.saves.selectedBackgroundIndex = selectedIndex; // Сохраняем выбранный индекс фона
                SetBackgroundById(index); // Устанавливаем новый фон
            }
            else
            {
                YG2.saves.selectedItemSkinIndex = index; // Сохраняем выбранный индекс скина
            }

            // Обновляем визуал для обоих списков
            UpdateAllGoodsStates(YG2.saves.backgrounds, selectedIndexBackgrounds);
            UpdateAllGoodsStates(YG2.saves.items, selectedIndexItems);

            YG2.SaveProgress(); // Сохраняем изменения
        }
        else
        {
            soundManager.PlayErrorSound();
        }
    }

    private bool TryPurchaseGoods(Goods goods)
    {
        int playerCoins = YG2.saves.GetCoins();
        if (goods.price > 0 && playerCoins >= goods.price)
        {
            YG2.saves.SubCoins(goods.price);
            goods.price = 0;
            return true;
        }
        return goods.price == 0;
    }

    private void UpdateAllGoodsStates(List<Goods> goodsList, int selectedIndex)
    {
        for (int i = 0; i < goodsList.Count; i++)
        {
            UpdateGoodsState(goodsList[i], i, selectedIndex);
        }
    }

    private void UpdateGoodsState(Goods goods, int index, int selectedIndex)
    {
        if (goods.uiImage != null)
        {
            goods.uiImage.sprite = (goods.price > 0) ? closedSprite :
                                   (index == selectedIndex) ? selectedSprite : availableSprite;
        }
        if (goods.uiText != null)
        {
            goods.uiText.text = (goods.price > 0) ? goods.price.ToString() : "";
        }
    }

    public void SetBackgroundById(int id = 0)
    {
        if (YG2.saves.backgrounds.Count == 0 || backgroundRenderer == null)
        {
            Debug.LogWarning("Backgrounds list is empty or backgroundRenderer is null!");
            return;
        }

        if (id >= 0 && id < YG2.saves.backgrounds.Count)
        {
            StartCoroutine(FadeOutAndIn(id));
        }
        else
        {
            Debug.LogWarning("Invalid background index!");
        }
    }

    private IEnumerator FadeOutAndIn(int index)
    {
        GameObject secondaryObject = new("SecondaryBackground");
        SpriteRenderer secondaryRenderer = secondaryObject.AddComponent<SpriteRenderer>();

        secondaryRenderer.sprite = YG2.saves.backgrounds[index].sprite;
        secondaryRenderer.transform.position = backgroundRenderer.transform.position;
        secondaryRenderer.transform.localScale = backgroundRenderer.transform.localScale;
        secondaryRenderer.sortingOrder = backgroundRenderer.sortingOrder - 1;

        UpdateUIColors(GetAverageBackgroundColor(secondaryRenderer), fadeDuration);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeDuration);
            backgroundRenderer.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        backgroundRenderer.sprite = YG2.saves.backgrounds[index].sprite;
        backgroundRenderer.color = Color.white;
        Destroy(secondaryObject);
    }

    private Color GetAverageBackgroundColor(SpriteRenderer renderer)
    {
        Texture2D texture = SpriteToTexture(renderer.sprite);
        if (texture == null) return Color.black;

        Color[] pixels = texture.GetPixels();
        if (pixels.Length == 0) return Color.black;

        float r = 0, g = 0, b = 0, a = 0;
        foreach (Color color in pixels)
        {
            r += color.r;
            g += color.g;
            b += color.b;
            a += color.a;
        }

        int count = pixels.Length;
        return new Color(r / count, g / count, b / count, a / count);
    }

    private Texture2D SpriteToTexture(Sprite sprite)
    {
        if (sprite == null) return null;

        RenderTexture rt = new RenderTexture(sprite.texture.width, sprite.texture.height, 0);
        Graphics.Blit(sprite.texture, rt);

        Texture2D newTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rt;
        newTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        newTexture.Apply();
        RenderTexture.active = null;

        rt.Release();
        Destroy(rt);

        return newTexture;
    }

    private void UpdateUIColors(Color backgroundColor, float duration)
    {
        float brightness = backgroundColor.grayscale;
        Color targetUIColor = brightness < 0.5f ? backgroundColor * 1.5f : backgroundColor * 0.7f;

        // Обновляем цвет UI элементов
        StartCoroutine(AnimateUIColorChange(targetUIColor, duration));

        // Обновляем цвет кнопок с другим оттенком
        Color buttonColor = new Color(
            Mathf.Clamp(backgroundColor.r * 0.8f, 0, 1),
            Mathf.Clamp(backgroundColor.g * 0.8f, 0, 1),
            Mathf.Clamp(backgroundColor.b * 0.8f, 0, 1),
            1
        );
        StartCoroutine(AnimateUIButtonColorChange(buttonColor, duration));
    }

    private IEnumerator AnimateUIColorChange(Color targetUIColor, float duration)
    {
        float elapsedTime = 0;
        Color[] startColors = new Color[uiElements.Length];

        for (int i = 0; i < uiElements.Length; i++)
            startColors[i] = uiElements[i].color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            for (int i = 0; i < uiElements.Length; i++)
                uiElements[i].color = Color.Lerp(startColors[i], targetUIColor, t);

            yield return null;
        }

        for (int i = 0; i < uiElements.Length; i++)
            uiElements[i].color = targetUIColor;
    }

    private IEnumerator AnimateUIButtonColorChange(Color targetColor, float duration)
    {
        float elapsedTime = 0;
        Color[] startColors = new Color[uiButtons.Length];

        for (int i = 0; i < uiButtons.Length; i++)
            startColors[i] = uiButtons[i].image.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            for (int i = 0; i < uiButtons.Length; i++)
                uiButtons[i].image.color = Color.Lerp(startColors[i], targetColor, t);

            yield return null;
        }

        for (int i = 0; i < uiButtons.Length; i++)
            uiButtons[i].image.color = targetColor;
    }
}

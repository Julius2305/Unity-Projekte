using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RayReciever : MonoBehaviour
{
    private MeshContainer container;
    private MeshController controller;

    private bool isActive;

    private void Start()
    {
        isActive = false;

        container = new MeshContainer(this.transform);
        controller = new MeshController(container);

        container.ApplyColor(ColorCalculator.BASE_COLOR);
    }

    private void FixedUpdate()
    {
        if (isActive)
            GetAndApplyDose();
    }

    private void Update()
    {
        if (UnityInput.Instance.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
            DoseInfo.Instance.Controller.SetSourceActiveText(isActive);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            float[] accumulatedDoses = controller.VerticeData.Select(x => x.Dose).ToArray();

            for (int i = 0; i < accumulatedDoses.Length; i++)
            {
                Debug.Log(accumulatedDoses[i]);
            }
        }
    }

    /// <summary>
    /// Necessary so that the gameObject can be exposed to the radiation.
    /// Performs all the needed steps for calculating and recieving the radiation.
    /// </summary>
    private void GetAndApplyDose()
    {
        if (RaySource.Instance == null)
            return;

        // update vertices
        controller.UpdateVertices(RaySource.Instance.transform.position);
        controller.SortOutUnhittedVertices(RaySource.Instance.RayTracer);

        // get doses
        float[] distances = RaySource.Instance.RayTracer.GetDistances(controller.GetRelevantVerticePositions());
        float[] addedDoses = DoseCalculator.Calculate(distances, RaySource.Instance.BaseEnergy, Time.fixedDeltaTime);

        // store doses
        controller.StoreDoses(addedDoses);

        // calculate colors and avg. dose
        float[] accumulatedDoses = controller.VerticeData.Select(x => x.Dose).ToArray();
        float avgDose = DoseCalculator.GetAVGDose(accumulatedDoses);
        Color32[] colors = ColorCalculator.Calculate(accumulatedDoses);

        // appy colors and avg. dose
        container.ApplyColors(colors);
        DoseInfo.Instance.Controller.SetAVGDose(avgDose);
    }
}
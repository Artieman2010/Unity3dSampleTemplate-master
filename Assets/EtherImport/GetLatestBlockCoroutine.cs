﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Unity.Rpc;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using System;


public class GetLatestBlockCoroutine : MonoBehaviour
{
    public string Url = "http://localhost:8545";
    public string UrlFull = "https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c";

    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        InputUrl.text = Url;
    }

    public void GetBlockNumberRequest()
    {
        StartCoroutine(GetBlockNumber());
    }

    public IEnumerator GetBlockNumber()
    {

       var blockNumberRequest = new EthBlockNumberUnityRequest(InputUrl.text);
 
       yield return blockNumberRequest.SendRequest();

        if (blockNumberRequest.Exception != null)
        {
            UnityEngine.Debug.Log(blockNumberRequest.Exception.Message);
        }
        else
        {
            ResultBlockNumber.text = blockNumberRequest.Result.Value.ToString();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

﻿/*
	* SPDX-License-Identifier: Apache-2.0
	* Copyright 2021 FINOS FDC3 contributors - see NOTICE file
	*/
    
using Finos.Fdc3.Backplane.DTO.Envelope;
using Finos.Fdc3.Backplane.DTO.FDC3;
using System;

namespace Finos.Fdc3.Backplane.Client.Utils
{
    internal class MessageEnvelopGenerator
    {
        public static MessageEnvelope GetMessageEnvelope(Context context, string channelId, AppIdentifier appIdentifier)
        {
            return new MessageEnvelope()
            {
                ActionType = Fdc3Action.Broadcast,
                Payload = new EnvelopeData() { Context = context, ChannelId = channelId },
                Meta = new EnvelopeMeta()
                {
                    Source = appIdentifier,
                    UniqueMessageId = Guid.NewGuid().ToString(),
                }
            };
        }
    }
}

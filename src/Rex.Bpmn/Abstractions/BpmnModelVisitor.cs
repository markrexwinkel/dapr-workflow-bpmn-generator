using Rex.Bpmn.Abstractions.Model;

namespace Rex.Bpmn.Abstractions
{
    public abstract class BpmnModelVisitor
    {
        protected virtual void VisitActivity(Activity activity)
        {
            activity.Properties.Visit(VisitBaseElement);
            activity.DataInputAssociations.Visit(VisitBaseElement);
            activity.DataOutputAssociations.Visit(VisitBaseElement);
            activity.ResourceRoles.Visit(VisitBaseElement);
            activity.LoopCharacteristics.VisitIfNotNull(VisitBaseElement);

            switch(activity)
            {
                case Task task:
                    VisitTask(task);
                    break;
                case SubProcess subProcess:
                    VisitSubProcess(subProcess);
                    break;
                case CallActivity callActivity:
                    VisitCallActivity(callActivity);
                    break;
            }
        }

        protected virtual void VisitAdHocSubProcess(AdHocSubProcess adHocSubProcess)
        {
            adHocSubProcess.CompletionCondition.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitArtifact(Artifact artifact)
        {
            switch(artifact)
            {
                case TextAnnotation textAnnotation:
                    VisitTextAnnotation(textAnnotation);
                    break;
                case Group group:
                    VisitGroup(group);
                    break;
                case Association association:
                    VisitAssociation(association);
                    break;
            }
        }

        protected virtual void VisitAssignment(Assignment assignment)
        {
            assignment.From.VisitIfNotNull(VisitBaseElementWithMixedContent);
            assignment.To.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitAssociation(Association association)
        {
        }

        protected virtual void VisitAuditing(Auditing auditing)
        {
        }

        
        
        public virtual void VisitBaseElement(BaseElement baseElement)
        {
            baseElement.Documentation.Visit(VisitDocumentation);
            VisitExtensionElements(baseElement.ExtensionElements);
            switch(baseElement)
            {
                case Relationship relationship:
                    VisitRelationship(relationship);
                    break;
                case ResourceParameter resourceParameter:
                    VisitResourceParameter(resourceParameter);
                    break;
                case Operation operation:
                    VisitOperation(operation);
                    break;
                case CorrelationPropertyRetrievalExpression correlationPropertyRetrievalExpression:
                    VisitCorrelationPropertyRetrievalExpression(correlationPropertyRetrievalExpression);
                    break;
                case ConversationLink conversationLink:
                    VisitConversationLink(conversationLink);
                    break;
                case MessageFlowAssociation messageFlowAssociation:
                    VisitMessageFlowAssociation(messageFlowAssociation);
                    break;
                case ConversationAssociation conversationAssociation:
                    VisitConversationAssociation(conversationAssociation);
                    break;
                case ConversationNode conversationNode:
                    VisitConversationNode(conversationNode);
                    break;
                case MessageFlow messageFlow:
                    VisitMessageFlow(messageFlow);
                    break;
                case ParticipantMultiplicity participantMultiplicity:
                    VisitParticipantMultiplicity(participantMultiplicity);
                    break;
                case Participant participant:
                    VisitParticipant(participant);
                    break;
                case CorrelationPropertyBinding correlationPropertyBinding:
                    VisitCorrelationPropertyBinding(correlationPropertyBinding);
                    break;
                case CorrelationSubscription correlationSubscription:
                    VisitCorrelationSubscription(correlationSubscription);
                    break;
                case InputOutputBinding inputOutputBinding:
                    VisitInputOutputBinding(inputOutputBinding);
                    break;
                case RootElement rootElement:
                    VisitRootElement(rootElement);
                    break;
                case ParticipantAssociation participantAssociation:
                    VisitParticipantAssociation(participantAssociation);
                    break;
                case CorrelationKey correlationKey:
                    VisitCorrelationKey(correlationKey);
                    break;
                case Rendering rendering:
                    VisitRendering(rendering);
                    break;
                case Lane lane:
                    VisitLane(lane);
                    break;
                case LaneSet laneSet:
                    VisitLaneSet(laneSet);
                    break;
                case LoopCharacteristics loopCharacteristics:
                    VisitLoopCharacteristics(loopCharacteristics);
                    break;
                case ResourceAssignmentExpression resourceAssignmentExpression:
                    VisitResourceAssignmentExpression(resourceAssignmentExpression);
                    break;
                case ResourceParameterBinding resourceParameterBinding:
                    VisitResourceParameterBinding(resourceParameterBinding);
                    break;
                case ResourceRole resourceRole:
                    VisitResourceRole(resourceRole);
                    break;
                case DataAssociation dataAssociation:
                    VisitDataAssociation(dataAssociation);
                    break;
                case Property property:
                    VisitProperty(property);
                    break;
                case OutputSet outputSet:
                    VisitOutputSet(outputSet);
                    break;
                case InputSet inputSet:
                    VisitInputSet(inputSet);
                    break;
                case DataOutput dataOutput:
                    VisitDataOutput(dataOutput);
                    break;
                case DataInput dataInput:
                    VisitDataInput(dataInput);
                    break;
                case InputOutputSpecification inputOutputSpecification:
                    VisitInputOutputSpecification(inputOutputSpecification);
                    break;
                case DataState dataState:
                    VisitDataState(dataState);
                    break;
                case Monitoring monitoring:
                    VisitMonitoring(monitoring);
                    break;
                case FlowElement flowElement:
                    VisitFlowElement(flowElement);
                    break;
                case ComplexBehaviorDefinition complexBehaviorDefinition:
                    VisitComplexBehaviorDefinition(complexBehaviorDefinition);
                    break;
                case CategoryValue categoryValue:
                    VisitCategoryValue(categoryValue);
                    break;
                case Auditing auditing:
                    VisitAuditing(auditing);
                    break;
                case Assignment assignment:
                    VisitAssignment(assignment);
                    break;
                case Artifact artifact:
                    VisitArtifact(artifact);
                    break;
            }
        }

        public virtual void VisitBaseElementWithMixedContent(BaseElementWithMixedContent baseElementWithMixedContent)
        {
            baseElementWithMixedContent.Documentation.Visit(VisitDocumentation);
            baseElementWithMixedContent.ExtensionElements.VisitIfNotNull(VisitExtensionElements);

            switch(baseElementWithMixedContent)
            {
                case Expression expression:
                    VisitExpression(expression);
                    break;
            }
        }

        protected virtual void VisitBoundaryEvent(BoundaryEvent boundaryEvent)
        { 
        }

        protected virtual void VisitBounds(Bounds bounds)
        {
        }

        protected virtual void VisitBpmnDiagram(BpmnDiagram bpmnDiagram)
        {
            bpmnDiagram.LabelStyles.Visit(VisitStyle);
            bpmnDiagram.Plane.VisitIfNotNull(VisitDiagramElement);
        }

        protected virtual void VisitBpmnEdge(BpmnEdge bpmnEdge)
        {
        }

        protected virtual void VisitBpmnLabel(BpmnLabel bpmnLabel)
        {
            
        }

        protected virtual void VisitBpmnLabelStyle(BpmnLabelStyle bpmnLabelStyle)
        {
            bpmnLabelStyle.Font.VisitIfNotNull(VisitFont);
        }

        protected virtual void VisitBpmnPlane(BpmnPlane bpmnPlane)
        {
        }

        protected virtual void VisitBpmnShape(BpmnShape bpmnShape)
        {
            bpmnShape.Label.VisitIfNotNull(VisitDiagramElement);
        }

        protected virtual void VisitBusinessRuleTask(BusinessRuleTask businessRuleTask)
        {
        }

        protected virtual void VisitCallableElement(CallableElement callableElement)
        {
            callableElement.IoBindings.Visit(VisitBaseElement);
            callableElement.IoSpecification.VisitIfNotNull(VisitBaseElement);
            
            switch(callableElement)
            {
                case Process process:
                    VisitProcess(process);
                    break;
                case GlobalTask globalTask:
                    VisitGlobalTask(globalTask);
                    break;
            }
        }

        protected virtual void VisitCallActivity(CallActivity callActivity)
        {
        }

        protected virtual void VisitCallChoreography(CallChoreography callChoreography)
        {
            callChoreography.ParticipantAssociations.Visit(VisitBaseElement);
        }

        protected virtual void VisitCallConversation(CallConversation callConversation)
        {
            callConversation.ParticipantAssocations.Visit(VisitBaseElement);
        }

        protected virtual void VisitCancelEventDefinition(CancelEventDefinition cancelEventDefinition)
        {
        }

        protected virtual void VisitCatchEvent(CatchEvent catchEvent)
        {
            catchEvent.DataOutputAssociations.Visit(VisitBaseElement);
            catchEvent.DataOutputs.Visit(VisitBaseElement);
            catchEvent.EventDefinitions.Visit(VisitBaseElement);
            catchEvent.OutputSet.VisitIfNotNull(VisitBaseElement);

            switch(catchEvent)
            {
                case StartEvent startEvent:
                    VisitStartEvent(startEvent);
                    break;
                case IntermediateCatchEvent intermediateCatchEvent:
                    VisitIntermediateCatchEvent(intermediateCatchEvent);
                    break;
                case BoundaryEvent boundaryEvent:
                    VisitBoundaryEvent(boundaryEvent);
                    break;
            }
        }

        protected virtual void VisitCategory(Category category)
        {
            category.Values.Visit(VisitBaseElement);
        }

        protected virtual void VisitCategoryValue(CategoryValue categoryValue)
        {
        }

        protected virtual void VisitChoreography(Choreography choreography)
        {
            choreography.FlowElements.Visit(VisitBaseElement);
            switch(choreography)
            {
                case GlobalChoreographyTask globalChoreographyTask:
                    VisitGlobalChoreographyTask(globalChoreographyTask);
                    break;
            }
        }

        protected virtual void VisitChoreographyActivity(ChoreographyActivity choreographyActivity)
        {
            choreographyActivity.CorrelationKeys.Visit(VisitBaseElement);
            switch(choreographyActivity)
            {
                case CallChoreography callChoreography:
                    VisitCallChoreography(callChoreography);
                    break;
                case ChoreographyTask choreographyTask:
                    VisitChoreographyTask(choreographyTask);
                    break;
                case SubChoreography subChoreograhy:
                    VisitSubChoreography(subChoreograhy);
                    break;
            }
        }

        protected virtual void VisitChoreographyTask(ChoreographyTask choreographyTask)
        {
        }

        protected virtual void VisitCollaboration(Collaboration collaboration)
        {
            collaboration.Artifacts.Visit(VisitBaseElement);
            collaboration.ConversationAssociations.Visit(VisitBaseElement);
            collaboration.ConversationLinks.Visit(VisitBaseElement);
            collaboration.ConversationNodes.Visit(VisitBaseElement);
            collaboration.CorrelationKeys.Visit(VisitBaseElement);
            collaboration.MessageFlowAssociations.Visit(VisitBaseElement);
            collaboration.MessageFlows.Visit(VisitBaseElement);
            collaboration.ParticipantAssociations.Visit(VisitBaseElement);
            collaboration.Participants.Visit(VisitBaseElement);

            switch(collaboration)
            {
                case Choreography choreography:
                    VisitChoreography(choreography);
                    break;
                case GlobalConversation globalConversation:
                    VisitGlobalConversation(globalConversation);
                    break;
            }
        }

        protected virtual void VisitCompensateEventDefinition(CompensateEventDefinition compensateEventDefinition)
        {
        }

        protected virtual void VisitComplexBehaviorDefinition(ComplexBehaviorDefinition complexBehaviorDefinition)
        {
            complexBehaviorDefinition.Condition.VisitIfNotNull(VisitBaseElementWithMixedContent);
            complexBehaviorDefinition.Event.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitComplexGateway(ComplexGateway complexGateway)
        {
            complexGateway.ActivationCondition.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitConditionalEventDefinition(ConditionalEventDefinition conditionalEventDefinition)
        {
            conditionalEventDefinition.Condition.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitConversation(Conversation conversation)
        {
        }

        protected virtual void VisitConversationAssociation(ConversationAssociation conversationAssociation)
        {
        }

        protected virtual void VisitConversationLink(ConversationLink conversationLink)
        {
        }
        
        protected virtual void VisitConversationNode(ConversationNode conversationNode)
        {
            conversationNode.CorrelationKeys.Visit(VisitBaseElement);
            switch(conversationNode)
            {
                case SubConversation subConversation:
                    VisitSubConversation(subConversation);
                    break;
                case Conversation conversation:
                    VisitConversation(conversation);
                    break;
                case CallConversation callConversation:
                    VisitCallConversation(callConversation);
                    break;
            }
        }

        protected virtual void VisitCorrelationKey(CorrelationKey correlationKey)
        {
        }

        protected virtual void VisitCorrelationProperty(CorrelationProperty correlationProperty)
        {
            correlationProperty.RetrievalExpressions.Visit(VisitBaseElement);
        }

        protected virtual void VisitCorrelationPropertyBinding(CorrelationPropertyBinding correlationPropertyBinding)
        {
        }

        protected virtual void VisitCorrelationPropertyRetrievalExpression(CorrelationPropertyRetrievalExpression correlationPropertyRetrievalExpression)
        {
            correlationPropertyRetrievalExpression.MessagePath.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitCorrelationSubscription(CorrelationSubscription correlationSubscription)
        {
            correlationSubscription.Bindings.Visit(VisitBaseElement);
        }

        protected virtual void VisitDataAssociation(DataAssociation dataAssociation)
        {
            dataAssociation.Assignments.Visit(VisitBaseElement);
            dataAssociation.Transformation.VisitIfNotNull(VisitBaseElementWithMixedContent);
            switch(dataAssociation)
            {
                case DataOutputAssociation dataOutputAssociation:
                    VisitDataOutputAssociation(dataOutputAssociation);
                    break;
                case DataInputAssociation dataInputAssociation:
                    VisitDataInputAssociation(dataInputAssociation);
                    break;
            }
        }

        protected virtual void VisitDataInput(DataInput dataInput)
        {
            dataInput.DataState.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitDataInputAssociation(DataInputAssociation dataInputAssociation)
        {
        }

        protected virtual void VisitDataObject(DataObject dataObject)
        {
            dataObject.DataState.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitDataObjectReference(DataObjectReference dataObjectReference)
        {
            dataObjectReference.DataState.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitDataOutput(DataOutput dataOutput)
        {
            dataOutput.DataState.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitDataOutputAssociation(DataOutputAssociation dataOutputAssociation)
        {
        }

        protected virtual void VisitDataState(DataState dataState)
        {
        }

        protected virtual void VisitDataStore(DataStore dataStore)
        {
            dataStore.DataState.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitDataStoreReference(DataStoreReference dataStoreReference)
        {
            dataStoreReference.DataState.VisitIfNotNull(VisitBaseElement);
        }

        public virtual void VisitDefinitions(Definitions definitions)
        {
            definitions.Imports.Visit(VisitImport);
            definitions.Extensions.Visit(VisitExtension);
            definitions.RootElements.Visit(VisitBaseElement);
            definitions.BpmnDiagrams.Visit(VisitDiagram);
            definitions.Relationships.Visit(VisitBaseElement);
        }

        public virtual void VisitDiagram(Diagram diagram)
        {
            switch(diagram)
            {
                case BpmnDiagram bpmnDiagram:
                    VisitBpmnDiagram(bpmnDiagram);
                    break;
            }
        }

        public virtual void VisitDiagramElement(DiagramElement diagramElement)
        {
            diagramElement.Extension.VisitIfNotNull(VisitDiagramElementExtension);
            switch(diagramElement)
            {
                case Edge edge:
                    VisitEdge(edge);
                    break;
                case Node node:
                    VisitNode(node);
                    break;
            }
        }

        protected virtual void VisitDiagramElementExtension(DiagramElementExtension diagramElementExtension)
        {
        }

        protected virtual void VisitDocumentation(Documentation documentation)
        {
        }

        protected virtual void VisitEdge(Edge edge)
        {
            edge.WayPoints.Visit(VisitPoint);
            switch(edge)
            {
                case LabeledEdge labeledEdge:
                    VisitLabeledEdge(labeledEdge);
                    break;
            }
        }

        protected virtual void VisitEndEvent(EndEvent endEvent)
        {
        }

        protected virtual void VisitEndPoint(EndPoint endPoint)
        {
        }

        protected virtual void VisitError(Error error)
        {
        }

        protected virtual void VisitErrorEventDefinition(ErrorEventDefinition errorEventDefinition)
        {
        }

        protected virtual void VisitEscalation(Escalation escalation)
        {
        }

        protected virtual void VisitEscalationEventDefinition(EscalationEventDefinition escalationEventDefinition)
        {
        }

        protected virtual void VisitEvent(Event @event)
        {
            @event.Properties.Visit(VisitBaseElement);
            switch(@event)
            {
                case ThrowEvent throwEvent:
                    VisitThrowEvent(throwEvent);
                    break;
                case CatchEvent catchEvent:
                    VisitCatchEvent(catchEvent);
                    break;
            }
        }

        protected virtual void VisitEventBasedGateway(EventBasedGateway eventBasedGateway)
        {
        }

        protected virtual void VisitEventDefinition(EventDefinition eventDefinition)
        {
            switch (eventDefinition)
            {
                case TimerEventDefinition timerEventDefintion:
                    VisitTimerEventDefinition(timerEventDefintion);
                    break;
                case TerminateEventDefinition terminateEventDefinition:
                    VisitTerminateEventDefinition(terminateEventDefinition);
                    break;
                case SignalEventDefinition signalEventDefinition:
                    VisitSignalEventDefinition(signalEventDefinition);
                    break;
                case MessageEventDefinition messageEventDefinition:
                    VisitMessageEventDefinition(messageEventDefinition);
                    break;
                case LinkEventDefinition linkEventDefinition:
                    VisitLinkEventDefinition(linkEventDefinition);
                    break;
                case EscalationEventDefinition escalationEventDefinition:
                    VisitEscalationEventDefinition(escalationEventDefinition);
                    break;
                case ErrorEventDefinition errorEventDefinition:
                    VisitErrorEventDefinition(errorEventDefinition);
                    break;
                case ConditionalEventDefinition conditionalEventDefinition:
                    VisitConditionalEventDefinition(conditionalEventDefinition);
                    break;
                case CompensateEventDefinition compensateEventDefinition:
                    VisitCompensateEventDefinition(compensateEventDefinition);
                    break;
                case CancelEventDefinition cancelEventDefinition:
                    VisitCancelEventDefinition(cancelEventDefinition);
                    break;
            }
        }

        protected virtual void VisitExclusiveGateway(ExclusiveGateway exclusiveGateway)
        {
        }

        protected virtual void VisitExpression(Expression expression)
        {
            switch(expression)
            {
                case FormalExpression formalExpression:
                    VisitFormalExpression(formalExpression);
                    break;
            }
        }

        protected virtual void VisitExtension(Extension extension)
        {
            extension.Documentation.Visit(VisitDocumentation);
        }

        protected virtual void VisitExtensionElements(ExtensionElements extensionElements)
        {
        }
        
        protected virtual void VisitFlowElement(FlowElement flowElement)
        {
            flowElement.Auditing.VisitIfNotNull(VisitBaseElement);
            flowElement.Monitoring.VisitIfNotNull(VisitBaseElement);
            
            switch(flowElement)
            {
                case SequenceFlow sequenceFlow:
                    VisitSequenceFlow(sequenceFlow);
                    break;
                case FlowNode flowNode:
                    VisitFlowNode(flowNode);
                    break;
                case DataStoreReference dataStoreReference:
                    VisitDataStoreReference(dataStoreReference);
                    break;
                case DataObjectReference dataObjectReference:
                    VisitDataObjectReference(dataObjectReference);
                    break;
                case DataObject dataObject:
                    VisitDataObject(dataObject);
                    break;
            }
        }      
       
        protected virtual void VisitFlowNode(FlowNode flowNode)
        {
            switch (flowNode)
            {
                case Gateway gateway:
                    VisitGateway(gateway);
                    break;
                case Event @event:
                    VisitEvent(@event);
                    break;
                case ChoreographyActivity choreographyActivity:
                    VisitChoreographyActivity(choreographyActivity);
                    break;
                case Activity activity:
                    VisitActivity(activity);
                    break;
            }
        }

        protected virtual void VisitFont(Font font)
        {
        }

        protected virtual void VisitFormalExpression(FormalExpression formalExpression)
        {
        }

        protected virtual void VisitGateway(Gateway gateway)
        {
            switch(gateway)
            {
                case ParallelGateway parallelGateway:
                    VisitParallelGateway(parallelGateway);
                    break;
                case InclusiveGateway inclusiveGateway:
                    VisitInclusiveGateway(inclusiveGateway);
                    break;
                case ExclusiveGateway exclusiveGateway:
                    VisitExclusiveGateway(exclusiveGateway);
                    break;
                case EventBasedGateway eventBasedGateway:
                    VisitEventBasedGateway(eventBasedGateway);
                    break;
                case ComplexGateway complexGateway:
                    VisitComplexGateway(complexGateway);
                    break;
            }
        }

        protected virtual void VisitGlobalBusinessRuleTask(GlobalBusinessRuleTask globalBusinessRuleTask)
        {
        }

        protected virtual void VisitGlobalChoreographyTask(GlobalChoreographyTask globalChoreographyTask)
        {
        }

        protected virtual void VisitGlobalConversation(GlobalConversation globalConversation)
        {
        }

        protected virtual void VisitGlobalManualTask(GlobalManualTask globalManualTask)
        {
        }

        protected virtual void VisitGlobalScriptTask(GlobalScriptTask globalScriptTask)
        {
        }

        protected virtual void VisitGlobalTask(GlobalTask globalTask)
        {
            globalTask.ResourceRoles.Visit(VisitBaseElement);
            
            switch (globalTask)
            {
                case GlobalUserTask globalUserTask:
                    VisitGlobalUserTask(globalUserTask);
                    break;
                case GlobalScriptTask globalScriptTask:
                    VisitGlobalScriptTask(globalScriptTask);
                    break;
                case GlobalManualTask globalManualTask:
                    VisitGlobalManualTask(globalManualTask);
                    break;
                case GlobalBusinessRuleTask globalBusinessRuleTask:
                    VisitGlobalBusinessRuleTask(globalBusinessRuleTask);
                    break;
            }
        }

        protected virtual void VisitGlobalUserTask(GlobalUserTask globalUserTask)
        {
            globalUserTask.Renderings.Visit(VisitBaseElement);
        }

        protected virtual void VisitGroup(Group group)
        {
        }

        protected virtual void VisitHumanPerformer(HumanPerformer humanPerformer)
        {
            switch(humanPerformer)
            {
                case PotentialOwner potentialOwner:
                    VisitPotentialOwner(potentialOwner);
                    break;
            }
        }

        protected virtual void VisitImplicitThrowEvent(ImplicitThrowEvent implicitThrowEvent)
        {
        }

        protected virtual void VisitImport(Import import)
        {
        }

        protected virtual void VisitInclusiveGateway(InclusiveGateway inclusiveGateway)
        {
        }

        protected virtual void VisitInputOutputBinding(InputOutputBinding inputOutputBinding)
        {
        }

        protected virtual void VisitInputOutputSpecification(InputOutputSpecification inputOutputSpecification)
        {
            inputOutputSpecification.DataInputs.Visit(VisitBaseElement);
            inputOutputSpecification.DataOutputs.Visit(VisitBaseElement);
            inputOutputSpecification.InputSets.Visit(VisitBaseElement);
            inputOutputSpecification.OutputSets.Visit(VisitBaseElement);
        }

        protected virtual void VisitInputSet(InputSet inputSet)
        {
        }

        protected virtual void VisitInterface(Interface @interface)
        {
            @interface.Operations.Visit(VisitBaseElement);
        }

        protected virtual void VisitIntermediateCatchEvent(IntermediateCatchEvent intermediateCatchEvent)
        {
        }

        protected virtual void VisitIntermediateThrowEvent(IntermediateThrowEvent intermediateThrowEvent)
        {
        }

        protected virtual void VisitItemDefinition(ItemDefinition itemDefinition)
        {
        }

        protected virtual void VisitLabel(Label label)
        {
            label.Bounds.VisitIfNotNull(VisitBounds);
            switch(label)
            {
                case BpmnLabel bpmnLabel:
                    VisitBpmnLabel(bpmnLabel);
                    break;
            }
        }

        protected virtual void VisitLabeledEdge(LabeledEdge labeledEdge)
        {
            switch(labeledEdge)
            {
                case BpmnEdge bpmnEdge:
                    VisitBpmnEdge(bpmnEdge);
                    break;
            }
        }

        protected virtual void VisitLabeledShape(LabeledShape labeledShape)
        {
            switch(labeledShape)
            {
                case BpmnShape bpmnShape:
                    VisitBpmnShape(bpmnShape);
                    break;
            }
        }

        protected virtual void VisitLane(Lane lane)
        {
            lane.ChildLaneSet.VisitIfNotNull(VisitBaseElement);
            lane.PartitionElement.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitLaneSet(LaneSet laneSet)
        {
            laneSet.Lanes.Visit(VisitBaseElement);
        }

        protected virtual void VisitLinkEventDefinition(LinkEventDefinition linkEventDefinition)
        {
        }
        
        protected virtual void VisitLoopCharacteristics(LoopCharacteristics loopCharacteristics)
        {
            switch(loopCharacteristics)
            {
                case StandardLoopCharacteristics standardLoopCharacteristics:
                    VisitStandardLoopCharacteristics(standardLoopCharacteristics);
                    break;
                case MultiInstanceLoopCharacteristics multiInstanceLoopCharacteristcs:
                    VisitMultiInstanceLoopCharacteristics(multiInstanceLoopCharacteristcs);
                    break;
            }
        }

        protected virtual void VisitManualTask(ManualTask manualTask)
        {
        }

        protected virtual void VisitMessage(Message message)
        {
        }

        protected virtual void VisitMessageEventDefinition(MessageEventDefinition messageEventDefinition)
        {
        }

        protected virtual void VisitMessageFlow(MessageFlow messageFlow)
        {
        }

        protected virtual void VisitMessageFlowAssociation(MessageFlowAssociation messageFlowAssociation)
        {
        }

        protected virtual void VisitMonitoring(Monitoring monitoring)
        {
        }

        protected virtual void VisitMultiInstanceLoopCharacteristics(MultiInstanceLoopCharacteristics multiInstanceLoopCharacteristics)
        {
            multiInstanceLoopCharacteristics.CompletionCondition.VisitIfNotNull(VisitBaseElementWithMixedContent);
            multiInstanceLoopCharacteristics.ComplexBehaviorDefinitions.Visit(VisitBaseElement);
            multiInstanceLoopCharacteristics.InputDataItem.VisitIfNotNull(VisitBaseElement);
            multiInstanceLoopCharacteristics.LoopCardinality.VisitIfNotNull(VisitBaseElementWithMixedContent);
            multiInstanceLoopCharacteristics.OutputDataItem.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitNode(Node node)
        {
            switch(node)
            {
                case Plane plane:
                    VisitPlane(plane);
                    break;
                case Label label:
                    VisitLabel(label);
                    break;
                case Shape shape:
                    VisitShape(shape);
                    break;
            }
        }

        protected virtual void VisitOperation(Operation operation)
        {
        }

        protected virtual void VisitOutputSet(OutputSet outputSet)
        {
        }

        protected virtual void VisitPerformer(Performer performer)
        {
            switch(performer)
            {
                case HumanPerformer humanPerformer:
                    VisitHumanPerformer(humanPerformer);
                    break;
            }
        }

        protected virtual void VisitParallelGateway(ParallelGateway parallelGateway)
        {
        }

        protected virtual void VisitParticipant(Participant participant)
        {
            participant.ParticipantMultiplicity.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitParticipantAssociation(ParticipantAssociation participantAssociation)
        {
        }

        protected virtual void VisitParticipantMultiplicity(ParticipantMultiplicity participantMultiplicity)
        {
        }

        protected virtual void VisitPartnerEntity(PartnerEntity partnerEntity)
        {
        }

        protected virtual void VisitPartnerRole(PartnerRole partnerRole)
        {
        }

        protected virtual void VisitPlane(Plane plane)
        {
            plane.Elements.Visit(VisitDiagramElement);
            switch(plane)
            {
                case BpmnPlane bpmnPlane:
                    VisitBpmnPlane(bpmnPlane);
                    break;
            }
        }

        protected virtual void VisitPoint(Point point)
        {
        }

        protected virtual void VisitPotentialOwner(PotentialOwner potentialOwner)
        {
        }

        protected virtual void VisitProcess(Process process)
        {
            process.Artifacts.Visit(VisitBaseElement);
            process.Auditing.VisitIfNotNull(VisitBaseElement);
            process.CorrelationSubscriptions.Visit(VisitBaseElement);
            process.FlowElements.Visit(VisitBaseElement);
            process.LaneSets.Visit(VisitBaseElement);
            process.Monitoring.VisitIfNotNull(VisitBaseElement);
            process.Properties.Visit(VisitBaseElement);
            process.ResourceRoles.Visit(VisitBaseElement);
        }

        protected virtual void VisitProperty(Property property)
        {
            property.DataState.VisitIfNotNull(VisitBaseElement);
        }

        protected virtual void VisitReceiveTask(ReceiveTask receiveTask)
        {
        }

        protected virtual void VisitRelationship(Relationship relationship)
        {   
        }

        protected virtual void VisitRendering(Rendering rendering)
        {
        }

        protected virtual void VisitResource(Resource resource)
        {
            resource.ResourceParameters.Visit(VisitBaseElement);
        }

        protected virtual void VisitResourceAssignmentExpression(ResourceAssignmentExpression resourceAssignmentExpression)
        {
            resourceAssignmentExpression.Expression.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitResourceParameter(ResourceParameter resourceParameter)
        {
        }

        protected virtual void VisitResourceParameterBinding(ResourceParameterBinding resourceParameterBinding)
        {
            resourceParameterBinding.Expression.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitResourceRole(ResourceRole resourceRole)
        {
            switch(resourceRole)
            {
                case Performer performer:
                    VisitPerformer(performer);
                    break;
            }
        }

        protected virtual void VisitRootElement(RootElement rootElement)
        {
            switch(rootElement)
            {
                case Signal signal:
                    VisitSignal(signal);
                    break;
                case Resource resource:
                    VisitResource(resource);
                    break;
                case PartnerRole partnerRole:
                    VisitPartnerRole(partnerRole);
                    break;
                case PartnerEntity partnerEntity:
                    VisitPartnerEntity(partnerEntity);
                    break;
                case Message message:
                    VisitMessage(message);
                    break;
                case ItemDefinition itemDefinition:
                    VisitItemDefinition(itemDefinition);
                    break;
                case Interface @interface:
                    VisitInterface(@interface);
                    break;
                case EventDefinition eventDefinition:
                    VisitEventDefinition(eventDefinition);
                    break;
                case Escalation escalation:
                    VisitEscalation(escalation);
                    break;
                case Error error:
                    VisitError(error);
                    break;
                case EndPoint endPoint:
                    VisitEndPoint(endPoint);
                    break;
                case DataStore dataStore:
                    VisitDataStore(dataStore);
                    break;
                case CorrelationProperty correlationProperty:
                    VisitCorrelationProperty(correlationProperty);
                    break;
                case Collaboration collaboration:
                    VisitCollaboration(collaboration);
                    break;
                case Category category:
                    VisitCategory(category);
                    break;
                case CallableElement callableElement:
                    VisitCallableElement(callableElement);
                    break;
            }
        }

        protected virtual void VisitScriptTask(ScriptTask scriptTask)
        {
        }

        protected virtual void VisitSendTask(SendTask sendTask)
        {
        }

        protected virtual void VisitSequenceFlow(SequenceFlow sequenceFlow)
        {
            sequenceFlow.ConditionExpression.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitServiceTask(ServiceTask serviceTask)
        {
        }

        protected virtual void VisitShape(Shape shape)
        {
            shape.Bounds.VisitIfNotNull(VisitBounds);
            switch(shape)
            {
                case LabeledShape labeledShape:
                    VisitLabeledShape(labeledShape);
                    break;
            }
        }

        protected virtual void VisitSignal(Signal signal)
        {
        }

        protected virtual void VisitSignalEventDefinition(SignalEventDefinition signalEventDefinition)
        {
        }

        protected virtual void VisitStandardLoopCharacteristics(StandardLoopCharacteristics standardLoopCharacteristics)
        {
            standardLoopCharacteristics.LoopCondition.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitStartEvent(StartEvent startEvent)
        {
        }

        protected virtual void VisitStyle(Style style)
        {
            switch(style)
            {
                case BpmnLabelStyle bpmnLabelStyle:
                    VisitBpmnLabelStyle(bpmnLabelStyle);
                    break;
            }
        }

        protected virtual void VisitSubChoreography(SubChoreography subChoreography)
        {
            subChoreography.Artifacts.Visit(VisitBaseElement);
            subChoreography.FlowElements.Visit(VisitBaseElement);
        }

        protected virtual void VisitSubConversation(SubConversation subConversation)
        {
            subConversation.ConversationNodes.Visit(VisitBaseElement);
        }

        protected virtual void VisitSubProcess(SubProcess subProcess)
        {
            subProcess.Artifacts.Visit(VisitBaseElement);
            subProcess.FlowElements.Visit(VisitBaseElement);
            subProcess.LaneSets.Visit(VisitBaseElement);
            switch(subProcess)
            {
                case Transaction transaction:
                    VisitTransaction(transaction);
                    break;
                case AdHocSubProcess adHocSubProcess:
                    VisitAdHocSubProcess(adHocSubProcess);
                    break;
            }
        }

        protected virtual void VisitTask(Task task)
        {
            switch(task)
            {
                case UserTask userTask:
                    VisitUserTask(userTask);
                    break;
                case ServiceTask serviceTask:
                    VisitServiceTask(serviceTask);
                    break;
                case SendTask sendTask:
                    VisitSendTask(sendTask);
                    break;
                case ScriptTask scriptTask:
                    VisitScriptTask(scriptTask);
                    break;
                case ReceiveTask receiveTask:
                    VisitReceiveTask(receiveTask);
                    break;
                case ManualTask manualTask:
                    VisitManualTask(manualTask);
                    break;
                case BusinessRuleTask businessRuleTask:
                    VisitBusinessRuleTask(businessRuleTask);
                    break;
            }
        }

        protected virtual void VisitTerminateEventDefinition(TerminateEventDefinition terminateEventDefinition)
        {
        }

        protected virtual void VisitTextAnnotation(TextAnnotation textAnnotaton)
        {
        }

        protected virtual void VisitThrowEvent(ThrowEvent throwEvent)
        {
            throwEvent.DataInputAssociations.Visit(VisitBaseElement);
            throwEvent.DataInputs.Visit(VisitBaseElement);
            throwEvent.EventDefinitions.Visit(VisitBaseElement);
            throwEvent.InputSet.VisitIfNotNull(VisitBaseElement);
            
            switch(throwEvent)
            {
                case IntermediateThrowEvent intermediateThrowEvent:
                    VisitIntermediateThrowEvent(intermediateThrowEvent);
                    break;
                case ImplicitThrowEvent implicitThrowEvent:
                    VisitImplicitThrowEvent(implicitThrowEvent);
                    break;
                case EndEvent endEvent:
                    VisitEndEvent(endEvent);
                    break;
            }
        }

        protected virtual void VisitTimerEventDefinition(TimerEventDefinition timerEventDefinition)
        {
            timerEventDefinition.Item.VisitIfNotNull(VisitBaseElementWithMixedContent);
        }

        protected virtual void VisitTransaction(Transaction transaction)
        {
        }

        protected virtual void VisitUserTask(UserTask userTask)
        {
            userTask.Renderings.Visit(VisitBaseElement);
        }
    }
}

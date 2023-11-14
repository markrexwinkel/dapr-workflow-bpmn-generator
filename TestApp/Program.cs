﻿using Rex.Bpmn.Model;
using Rex.Dapr.Workflow.Bpmn;
using Rex.Dapr.Workflow.Bpmn.Model;
using System.Text;
using System.Xml.Serialization;

namespace TestApp
{
    [XmlType("tTest", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("test", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Test
    {
        [XmlElement("flow")]
        public SequenceFlow Flow { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var s = new XmlSerializer(typeof(Test));
            var test = new Test
            {
                Flow = new SequenceFlow
                {
                    Id = "id",
                    Name = "name",
                    SourceRef = "sourceRef",
                    TargetRef = "targetRef",
                    ConditionExpression = new FormalExpression { Text = new System.Collections.ObjectModel.Collection<string>() { "formalExpression" } }
                }
            };
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            s.Serialize(sw, test);
            sw.Flush();
            var t = sb.ToString();
            var xml = """
                <?xml version="1.0" encoding="UTF-8"?>
                <bpmn:definitions xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:camunda="http://camunda.org/schema/1.0/bpmn" xmlns:dapr="https://dapr.io/schema/1.0/bpmn" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:modeler="http://camunda.org/schema/modeler/1.0" id="Definitions_15xzess" targetNamespace="http://bpmn.io/schema/bpmn" exporter="Camunda Modeler" exporterVersion="5.16.0" modeler:executionPlatform="Camunda Platform" modeler:executionPlatformVersion="7.17.0">
                  <bpmn:process id="LoanApplication" name="Loan Application" isExecutable="true" camunda:versionTag="1.0">
                    <bpmn:extensionElements>
                      <dapr:inputOutput>
                        <dapr:inputParameter name="ApplicantName" type="string"/>
                        <dapr:inputParameter name="LoanAmount" type="decimal"/>
                        <dapr:inputParameter name="YearlyGrossSalary" type="decimal"/>
                        <dapr:outputParameter name="Approved" type="bool" />
                      </dapr:inputOutput>
                    </bpmn:extensionElements>
                    <bpmn:startEvent id="LoanApplicationReceived" name="Loan Application Received">
                      <bpmn:outgoing>Flow_03d6nsm</bpmn:outgoing>
                      <bpmn:messageEventDefinition id="MessageEventDefinition_1mbsomi" />
                    </bpmn:startEvent>
                    <bpmn:sequenceFlow id="Flow_03d6nsm" sourceRef="LoanApplicationReceived" targetRef="DetermineExistingCustomer" />
                    <bpmn:exclusiveGateway id="gwy_ExistingCustomer" name="Existing Customer?">
                      <bpmn:incoming>Flow_0ooevif</bpmn:incoming>
                      <bpmn:outgoing>flw_ExistingCustomer_Yes</bpmn:outgoing>
                      <bpmn:outgoing>flw_ExistingCustomer_No</bpmn:outgoing>
                    </bpmn:exclusiveGateway>
                    <bpmn:sequenceFlow id="Flow_0ooevif" sourceRef="DetermineExistingCustomer" targetRef="gwy_ExistingCustomer" />
                    <bpmn:sequenceFlow id="flw_ExistingCustomer_Yes" name="Yes" sourceRef="gwy_ExistingCustomer" targetRef="Gateway_02c1v2m" />
                    <bpmn:sequenceFlow id="flw_ExistingCustomer_No" name="No" sourceRef="gwy_ExistingCustomer" targetRef="act_Registerropect" />
                    <bpmn:exclusiveGateway id="Gateway_02c1v2m">
                      <bpmn:incoming>Flow_112zwdj</bpmn:incoming>
                      <bpmn:incoming>flw_ExistingCustomer_Yes</bpmn:incoming>
                      <bpmn:outgoing>Flow_09qxsbb</bpmn:outgoing>
                    </bpmn:exclusiveGateway>
                    <bpmn:sequenceFlow id="Flow_112zwdj" sourceRef="act_Registerropect" targetRef="Gateway_02c1v2m" />
                    <bpmn:sequenceFlow id="Flow_09qxsbb" sourceRef="Gateway_02c1v2m" targetRef="act_DetermineRiskProfile" />
                    <bpmn:sequenceFlow id="Flow_0wj7r5b" sourceRef="act_DetermineRiskProfile" targetRef="Gateway_13kw37t" />
                    <bpmn:exclusiveGateway id="gwy_LoanApproved" name="Loan &#10;Approved">
                      <bpmn:incoming>Flow_07i3tg5</bpmn:incoming>
                      <bpmn:incoming>Flow_1ff5vom</bpmn:incoming>
                      <bpmn:outgoing>flw_LoanApproved_Yes</bpmn:outgoing>
                      <bpmn:outgoing>flw_LoanApproved_No</bpmn:outgoing>
                    </bpmn:exclusiveGateway>
                    <bpmn:sequenceFlow id="Flow_07i3tg5" sourceRef="act_AssessApplication" targetRef="gwy_LoanApproved" />
                    <bpmn:sequenceFlow id="flw_LoanApproved_Yes" name="Yes" sourceRef="gwy_LoanApproved" targetRef="act_SendProposal" />
                    <bpmn:sequenceFlow id="flw_LoanApproved_No" name="No" sourceRef="gwy_LoanApproved" targetRef="act_SendRejectionLetter" />
                    <bpmn:exclusiveGateway id="Gateway_10ptwt1">
                      <bpmn:incoming>Flow_0rj05k4</bpmn:incoming>
                      <bpmn:incoming>Flow_13gy3ar</bpmn:incoming>
                      <bpmn:incoming>Flow_0ostnmy</bpmn:incoming>
                      <bpmn:outgoing>Flow_0ohm3yk</bpmn:outgoing>
                    </bpmn:exclusiveGateway>
                    <bpmn:endEvent id="Event_0m2nbrj">
                      <bpmn:incoming>Flow_0ohm3yk</bpmn:incoming>
                    </bpmn:endEvent>
                    <bpmn:sequenceFlow id="Flow_0ohm3yk" sourceRef="Gateway_10ptwt1" targetRef="Event_0m2nbrj" />
                    <bpmn:sequenceFlow id="Flow_0ragejw" sourceRef="act_RegisterContract" targetRef="act_SendContract" />
                    <bpmn:sequenceFlow id="Flow_0rj05k4" sourceRef="act_SendContract" targetRef="Gateway_10ptwt1" />
                    <bpmn:userTask id="act_AssessApplication" name="Assess Application">
                      <bpmn:incoming>Flow_1bka56x</bpmn:incoming>
                      <bpmn:outgoing>Flow_07i3tg5</bpmn:outgoing>
                    </bpmn:userTask>
                    <bpmn:serviceTask id="act_DetermineRiskProfile" name="Determine&#10;Risk&#10;Profile">
                      <bpmn:incoming>Flow_09qxsbb</bpmn:incoming>
                      <bpmn:outgoing>Flow_0wj7r5b</bpmn:outgoing>
                    </bpmn:serviceTask>
                    <bpmn:serviceTask id="act_Registerropect" name="Register Customer">
                      <bpmn:incoming>flw_ExistingCustomer_No</bpmn:incoming>
                      <bpmn:outgoing>Flow_112zwdj</bpmn:outgoing>
                    </bpmn:serviceTask>
                    <bpmn:serviceTask id="DetermineExistingCustomer" name="Determine Existing Customer">
                      <bpmn:extensionElements>
                        <dapr:inputOutput>
                          <dapr:inputParameter name="ApplicantName" type="string" ref="ApplicantName" scope="Global"/>
                          <dapr:outputParameter name="CustomerId" type="string"/>
                          <dapr:outputParameter name="CustomerName" type="string"/>
                          <dapr:outputParameter name="OutstandingAmount" type="decimal"/>
                          <dapr:outputParameter name="HasDefaulted" type="bool"/>
                        </dapr:inputOutput>
                      </bpmn:extensionElements>
                      <bpmn:incoming>Flow_03d6nsm</bpmn:incoming>
                      <bpmn:outgoing>Flow_0ooevif</bpmn:outgoing>
                    </bpmn:serviceTask>
                    <bpmn:serviceTask id="act_RegisterContract" name="Register Contract">
                      <bpmn:incoming>Flow_0r0jszy</bpmn:incoming>
                      <bpmn:outgoing>Flow_0ragejw</bpmn:outgoing>
                    </bpmn:serviceTask>
                    <bpmn:serviceTask id="act_SendRejectionLetter" name="Send&#10;Rejection&#10;Letter">
                      <bpmn:incoming>flw_LoanApproved_No</bpmn:incoming>
                      <bpmn:outgoing>Flow_13gy3ar</bpmn:outgoing>
                    </bpmn:serviceTask>
                    <bpmn:serviceTask id="act_SendContract" name="Send&#10;Contract">
                      <bpmn:incoming>Flow_0ragejw</bpmn:incoming>
                      <bpmn:outgoing>Flow_0rj05k4</bpmn:outgoing>
                    </bpmn:serviceTask>
                    <bpmn:sequenceFlow id="Flow_04247nz" sourceRef="act_SendProposal" targetRef="Activity_07g5v2k" />
                    <bpmn:serviceTask id="act_SendProposal" name="Send&#10;Proposal">
                      <bpmn:incoming>flw_LoanApproved_Yes</bpmn:incoming>
                      <bpmn:outgoing>Flow_04247nz</bpmn:outgoing>
                    </bpmn:serviceTask>
                    <bpmn:receiveTask id="Activity_07g5v2k" name="Receive Customer Decision">
                      <bpmn:incoming>Flow_04247nz</bpmn:incoming>
                      <bpmn:outgoing>Flow_1tdhbnz</bpmn:outgoing>
                    </bpmn:receiveTask>
                    <bpmn:exclusiveGateway id="gwy_ProposalAccepted" name="Proposal Accepted?">
                      <bpmn:incoming>Flow_1tdhbnz</bpmn:incoming>
                      <bpmn:incoming>Flow_01jwrz3</bpmn:incoming>
                      <bpmn:outgoing>Flow_0r0jszy</bpmn:outgoing>
                      <bpmn:outgoing>Flow_0ostnmy</bpmn:outgoing>
                    </bpmn:exclusiveGateway>
                    <bpmn:sequenceFlow id="Flow_1tdhbnz" sourceRef="Activity_07g5v2k" targetRef="gwy_ProposalAccepted" />
                    <bpmn:sequenceFlow id="Flow_1nx6run" sourceRef="evt_Timeout" targetRef="act_ContactCustomer" />
                    <bpmn:sequenceFlow id="Flow_01jwrz3" sourceRef="act_ContactCustomer" targetRef="gwy_ProposalAccepted" />
                    <bpmn:userTask id="act_ContactCustomer" name="Contact Customer">
                      <bpmn:incoming>Flow_1nx6run</bpmn:incoming>
                      <bpmn:outgoing>Flow_01jwrz3</bpmn:outgoing>
                    </bpmn:userTask>
                    <bpmn:boundaryEvent id="evt_Timeout" name="14d" attachedToRef="Activity_07g5v2k">
                      <bpmn:outgoing>Flow_1nx6run</bpmn:outgoing>
                      <bpmn:timerEventDefinition id="TimerEventDefinition_1xu7zys">
                        <bpmn:timeDuration xsi:type="bpmn:tFormalExpression">P14D</bpmn:timeDuration>
                      </bpmn:timerEventDefinition>
                    </bpmn:boundaryEvent>
                    <bpmn:sequenceFlow id="Flow_13gy3ar" sourceRef="act_SendRejectionLetter" targetRef="Gateway_10ptwt1" />
                    <bpmn:sequenceFlow id="Flow_0r0jszy" name="Yes" sourceRef="gwy_ProposalAccepted" targetRef="act_RegisterContract" />
                    <bpmn:sequenceFlow id="Flow_0ostnmy" name="No" sourceRef="gwy_ProposalAccepted" targetRef="Gateway_10ptwt1" />
                    <bpmn:exclusiveGateway id="Gateway_13kw37t" name="Risk Class&#10;3 or higher?">
                      <bpmn:incoming>Flow_0wj7r5b</bpmn:incoming>
                      <bpmn:outgoing>Flow_1bka56x</bpmn:outgoing>
                      <bpmn:outgoing>Flow_1ff5vom</bpmn:outgoing>
                    </bpmn:exclusiveGateway>
                    <bpmn:sequenceFlow id="Flow_1bka56x" name="Yes" sourceRef="Gateway_13kw37t" targetRef="act_AssessApplication" />
                    <bpmn:sequenceFlow id="Flow_1ff5vom" name="No" sourceRef="Gateway_13kw37t" targetRef="gwy_LoanApproved" />
                    <bpmn:textAnnotation id="TextAnnotation_1chzuc1">
                      <bpmn:text>Loan is automatically approved</bpmn:text>
                    </bpmn:textAnnotation>
                    <bpmn:association id="Association_0id1j0f" sourceRef="Flow_1ff5vom" targetRef="TextAnnotation_1chzuc1" />
                  </bpmn:process>
                  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
                    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="LoanApplication">
                      <bpmndi:BPMNShape id="Event_1wopqui_di" bpmnElement="LoanApplicationReceived">
                        <dc:Bounds x="182" y="222" width="36" height="36" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="160" y="265" width="81" height="27" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Gateway_041ehgc_di" bpmnElement="gwy_ExistingCustomer" isMarkerVisible="true">
                        <dc:Bounds x="395" y="215" width="50" height="50" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="393" y="178" width="54" height="27" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Gateway_02c1v2m_di" bpmnElement="Gateway_02c1v2m" isMarkerVisible="true">
                        <dc:Bounds x="545" y="215" width="50" height="50" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Gateway_0ivk37j_di" bpmnElement="gwy_LoanApproved" isMarkerVisible="true">
                        <dc:Bounds x="1015" y="215" width="50" height="50" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="986" y="256" width="47" height="27" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Gateway_10ptwt1_di" bpmnElement="Gateway_10ptwt1" isMarkerVisible="true">
                        <dc:Bounds x="1835" y="215" width="50" height="50" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Event_0m2nbrj_di" bpmnElement="Event_0m2nbrj">
                        <dc:Bounds x="1912" y="222" width="36" height="36" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_1sdl5j2_di" bpmnElement="act_AssessApplication">
                        <dc:Bounds x="860" y="200" width="100" height="80" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_13atezs_di" bpmnElement="act_DetermineRiskProfile">
                        <dc:Bounds x="620" y="200" width="100" height="80" />
                        <bpmndi:BPMNLabel />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_18056q4_di" bpmnElement="act_Registerropect">
                        <dc:Bounds x="450" y="300" width="100" height="80" />
                        <bpmndi:BPMNLabel />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_18mey96_di" bpmnElement="DetermineExistingCustomer">
                        <dc:Bounds x="260" y="200" width="100" height="80" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_15eh99m_di" bpmnElement="act_RegisterContract">
                        <dc:Bounds x="1550" y="200" width="100" height="80" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_1vnkmoy_di" bpmnElement="act_SendRejectionLetter">
                        <dc:Bounds x="1110" y="370" width="100" height="80" />
                        <bpmndi:BPMNLabel />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_093a9bg_di" bpmnElement="act_SendContract">
                        <dc:Bounds x="1700" y="200" width="100" height="80" />
                        <bpmndi:BPMNLabel />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_1cwxtdu_di" bpmnElement="act_SendProposal">
                        <dc:Bounds x="1110" y="200" width="100" height="80" />
                        <bpmndi:BPMNLabel />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_09eqazx_di" bpmnElement="Activity_07g5v2k">
                        <dc:Bounds x="1260" y="200" width="100" height="80" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Gateway_100kcjl_di" bpmnElement="gwy_ProposalAccepted" isMarkerVisible="true">
                        <dc:Bounds x="1435" y="215" width="50" height="50" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="1471" y="256" width="52" height="27" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Activity_12d568y_di" bpmnElement="act_ContactCustomer">
                        <dc:Bounds x="1330" y="300" width="100" height="80" />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Gateway_13kw37t_di" bpmnElement="Gateway_13kw37t" isMarkerVisible="true">
                        <dc:Bounds x="755" y="215" width="50" height="50" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="750" y="272" width="59" height="27" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="TextAnnotation_1chzuc1_di" bpmnElement="TextAnnotation_1chzuc1">
                        <dc:Bounds x="890" y="80" width="160" height="42" />
                        <bpmndi:BPMNLabel />
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNShape id="Event_1gsraxm_di" bpmnElement="evt_Timeout">
                        <dc:Bounds x="1272" y="262" width="36" height="36" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="1264" y="298" width="19" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNShape>
                      <bpmndi:BPMNEdge id="Flow_03d6nsm_di" bpmnElement="Flow_03d6nsm">
                        <di:waypoint x="218" y="240" />
                        <di:waypoint x="260" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0ooevif_di" bpmnElement="Flow_0ooevif">
                        <di:waypoint x="360" y="240" />
                        <di:waypoint x="395" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0erhxgn_di" bpmnElement="flw_ExistingCustomer_Yes">
                        <di:waypoint x="445" y="240" />
                        <di:waypoint x="545" y="240" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="451" y="222" width="18" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0fmj3j5_di" bpmnElement="flw_ExistingCustomer_No">
                        <di:waypoint x="420" y="265" />
                        <di:waypoint x="420" y="340" />
                        <di:waypoint x="450" y="340" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="428" y="274" width="15" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_112zwdj_di" bpmnElement="Flow_112zwdj">
                        <di:waypoint x="550" y="340" />
                        <di:waypoint x="570" y="340" />
                        <di:waypoint x="570" y="265" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_09qxsbb_di" bpmnElement="Flow_09qxsbb">
                        <di:waypoint x="595" y="240" />
                        <di:waypoint x="620" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0wj7r5b_di" bpmnElement="Flow_0wj7r5b">
                        <di:waypoint x="720" y="240" />
                        <di:waypoint x="755" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_07i3tg5_di" bpmnElement="Flow_07i3tg5">
                        <di:waypoint x="960" y="240" />
                        <di:waypoint x="1015" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_1331440_di" bpmnElement="flw_LoanApproved_Yes">
                        <di:waypoint x="1065" y="240" />
                        <di:waypoint x="1110" y="240" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="1063" y="222" width="18" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0l2d7vt_di" bpmnElement="flw_LoanApproved_No">
                        <di:waypoint x="1040" y="265" />
                        <di:waypoint x="1040" y="410" />
                        <di:waypoint x="1110" y="410" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="1052" y="272" width="15" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0ohm3yk_di" bpmnElement="Flow_0ohm3yk">
                        <di:waypoint x="1885" y="240" />
                        <di:waypoint x="1912" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0ragejw_di" bpmnElement="Flow_0ragejw">
                        <di:waypoint x="1650" y="240" />
                        <di:waypoint x="1700" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0rj05k4_di" bpmnElement="Flow_0rj05k4">
                        <di:waypoint x="1800" y="240" />
                        <di:waypoint x="1835" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_04247nz_di" bpmnElement="Flow_04247nz">
                        <di:waypoint x="1210" y="240" />
                        <di:waypoint x="1260" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_1tdhbnz_di" bpmnElement="Flow_1tdhbnz">
                        <di:waypoint x="1360" y="240" />
                        <di:waypoint x="1435" y="240" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_1nx6run_di" bpmnElement="Flow_1nx6run">
                        <di:waypoint x="1290" y="298" />
                        <di:waypoint x="1290" y="340" />
                        <di:waypoint x="1330" y="340" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_01jwrz3_di" bpmnElement="Flow_01jwrz3">
                        <di:waypoint x="1430" y="340" />
                        <di:waypoint x="1460" y="340" />
                        <di:waypoint x="1460" y="265" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_13gy3ar_di" bpmnElement="Flow_13gy3ar">
                        <di:waypoint x="1210" y="410" />
                        <di:waypoint x="1860" y="410" />
                        <di:waypoint x="1860" y="265" />
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0r0jszy_di" bpmnElement="Flow_0r0jszy">
                        <di:waypoint x="1485" y="240" />
                        <di:waypoint x="1550" y="240" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="1488" y="222" width="18" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_0ostnmy_di" bpmnElement="Flow_0ostnmy">
                        <di:waypoint x="1460" y="215" />
                        <di:waypoint x="1460" y="150" />
                        <di:waypoint x="1860" y="150" />
                        <di:waypoint x="1860" y="215" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="1432" y="191" width="15" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_1bka56x_di" bpmnElement="Flow_1bka56x">
                        <di:waypoint x="805" y="240" />
                        <di:waypoint x="860" y="240" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="807" y="222" width="18" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Flow_1ff5vom_di" bpmnElement="Flow_1ff5vom">
                        <di:waypoint x="780" y="215" />
                        <di:waypoint x="780" y="150" />
                        <di:waypoint x="1040" y="150" />
                        <di:waypoint x="1040" y="215" />
                        <bpmndi:BPMNLabel>
                          <dc:Bounds x="752" y="191" width="15" height="14" />
                        </bpmndi:BPMNLabel>
                      </bpmndi:BPMNEdge>
                      <bpmndi:BPMNEdge id="Association_0id1j0f_di" bpmnElement="Association_0id1j0f">
                        <di:waypoint x="910" y="150" />
                        <di:waypoint x="925" y="122" />
                      </bpmndi:BPMNEdge>
                    </bpmndi:BPMNPlane>
                  </bpmndi:BPMNDiagram>
                </bpmn:definitions>
                """;
            
            var s2 = new XmlSerializer(typeof(Definitions));
            var t2 = (Definitions)s2.Deserialize(new StringReader(xml));
            var d = (Definitions)s2.Deserialize(File.OpenRead("LoanApplication.bpmn.old"));
            var p = d.RootElements.OfType<Process>().FirstOrDefault();
            //var io = p.GetExtensionElement<DaprInputOutput>();
            Console.WriteLine("Hello, World!");
        }
    }
}

using Rex.Bpmn.Model;

namespace Rex.Bpmn
{
    public class ElementFinder : BpmnModelVisitor
    {
        private string _id;
        private BaseElement _baseElement;
        
        public BaseElement FindBaseElement(Definitions definitions, string id)
        {
            _id = id;
            _baseElement = null;
            VisitDefinitions(definitions);
            return _baseElement;
        }

        public override void VisitBaseElement(BaseElement baseElement)
        {
            if(baseElement.Id == _id)
            {
                _baseElement = baseElement;
                return;
            }
            base.VisitBaseElement(baseElement);
        }
    }
}

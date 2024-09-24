import Header from '../../Header';
import Footer from '../../Footer';
import './styles.css';
import RegistrationOnEventForm from "../../forms/RegistrationOnEventForm";
import {useParams} from "react-router-dom";




const RegistrationOnEventPage = () => {
    const { id } = useParams<{ id: string }>();

    return ( 
        <div className='wrapper'>
      <Header />
      <div className='content'>
          {id ?<RegistrationOnEventForm id={id} /> : null}
      </div>
      <Footer />
    </div>
     );
}
 
export default RegistrationOnEventPage;
import Header from '../../Header';
import Footer from '../../Footer';
import RegistrationForm from "../../forms/RegisterForm";
import './styles.css';



const RegisterPage = () => {
    return ( 
        <div className='wrapper'>
      <Header />
      <div className='content'>
        <RegistrationForm />
      </div>
      <Footer />
    </div>
     );
}
 
export default RegisterPage;
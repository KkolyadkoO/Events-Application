import Header from '../../Header';
import Footer from '../../Footer';
import LoginForm from "../../forms/LoginForm";
import './styles.css';


const LoginPage = () => {
    return ( 
        <div className='wrapper'>
      <Header />
      <div className='content'>
        <LoginForm />
      </div>
      <Footer />
    </div>
     );
}
 
export default LoginPage;